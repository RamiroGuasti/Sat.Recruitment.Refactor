using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using Sat.Recruitment.DataAccess.Interfaces;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Entities.Base;

namespace Sat.Recruitment.DataAccess.Persistence.Context
{
    public class DataContext : DbContext, IDataContext
    {
        private static readonly string testConnection = "Data Source=localhost,10344\\MSSQLSERVER;Initial Catalog=SatTest;User Id=ramiro;Password=ramiro;MultipleActiveResultSets=true;";

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SatDB"]?.ToString() ?? testConnection;

        #region Propago Entities

        public virtual IDbSet<User> User { get; set; }

        #endregion

        public DataContext() : base(nameOrConnectionString: connectionString)
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

            Database.SetInitializer<DataContext>(null);

            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 120;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddAuditData();

            var result = base.SaveChanges();

            return result;
        }

        public IList<TEntity> GetChangeTrakerEntities<TEntity>(EntityState entityState) where TEntity : BaseEntity
        {
            return ChangeTracker.Entries().Where(x => x.Entity is TEntity && x.State == entityState).Select(x => (TEntity)x.Entity).ToList();
        }

        public IList<TEntity> ExecuteQuery<TEntity>(string command, params SqlParameter[] parameters)
        {
            return Database.SqlQuery<TEntity>(command, parameters).ToList();
        }

        public int ExecuteNonQuery(string command, params SqlParameter[] parameters)
        {
            return Database.ExecuteSqlCommand(command, parameters);
        }

        #region Base Entity Audit

        private void AddAuditData()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var loggedUser = "ramiro"; // get from request

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity) entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((BaseEntity) entity.Entity).CreatedBy = string.IsNullOrEmpty(((BaseEntity)entity.Entity).CreatedBy) ? loggedUser : ((BaseEntity)entity.Entity).CreatedBy;

                    ((BaseEntity) entity.Entity).ModifiedBy = null;
                    ((BaseEntity) entity.Entity).ModifiedDate = null;
                }
                else
                {
                    entity.Property("CreatedDate").IsModified = false;
                    entity.Property("CreatedBy").IsModified = false;

                    ((BaseEntity) entity.Entity).ModifiedDate = DateTime.UtcNow;
                    ((BaseEntity) entity.Entity).ModifiedBy = loggedUser;
                }
            }
        }

        #endregion

    }
}