using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Net.Utils.CustomException;
using Common.Net.Utils.CustomException.Base;
using Sat.Recruitment.Business.Interfaces.Base;
using Sat.Recruitment.DataAccess.Interfaces;
using Sat.Recruitment.Domain.Entities.Base;

namespace Sat.Recruitment.Business.Services.Base
{
    public class BaseEntityService<TEntity> : BaseService, IBaseEntityService<TEntity> where TEntity : BaseEntity
    {
        protected IList<ErrorContentItem> validationErrors = new List<ErrorContentItem>();

        public BaseEntityService(IDataContext dataContext) : base(dataContext)
        {
        }

        public virtual IQueryable<TEntity> GetBaseQueryableCollection(bool asNoTracking = true)
        {
            var query = GetBaseDataSet().Where(x => !x.IsDeleted);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public virtual IQueryable<TEntity> Get(IEnumerable<int> ids, bool asNoTracking = true)
        {
            var query = GetBaseQueryableCollection(false).Where(x => ids.Contains(x.Id));

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public virtual TEntity Get(int id, bool asNoTracking = true)
        {
            var item = GetBaseQueryableCollection(asNoTracking).SingleOrDefault(x => x.Id == id);

            if (item == null) throw new BusinessException(CustomErrorCode.Global_InvalidItem);

            return item;
        }
        
        public virtual int Add(TEntity item, bool saveChanges = true)
        {
            if (item.Id != 0) throw new BusinessException(CustomErrorCode.Global_InvalidItem);

            ExecuteValidationsChecker(item);

            GetBaseDataSet().Add(item);

            if (saveChanges) dataContext.SaveChanges();

            return item.Id;
        }

        public virtual void Update(TEntity item, bool saveChanges = true)
        {
            if (item.Id == 0) throw new BusinessException(CustomErrorCode.Global_UpdateItemIdValidation);
            
            ExecuteValidationsChecker(item);

            var dbEntity = Get(item.Id, false);

            var dbContext = dataContext as DbContext;
            
            dbContext.Entry(dbEntity).CurrentValues.SetValues(item);
            
            if (saveChanges) dataContext.SaveChanges();
        }

        public virtual void Delete(int id, bool saveChanges = true)
        {
            var dbEntity = Get(id, false);

            dbEntity.IsDeleted = true;
           
            if (saveChanges) dataContext.SaveChanges();
        }

        public void SpicySaveChanges()
        {
            dataContext.SaveChanges();
        }

        protected virtual void BusinessValidations(TEntity item)
        {
        }

        protected void AddBusinessValidation(bool validationResult, CustomErrorCode customErrorCode, params string[] additionalInfo)
        {
            if (validationResult) validationErrors.Add(new ErrorContentItem(customErrorCode, additionalInfo));
        }

        protected void ExecuteValidationsChecker(TEntity item)
        {
            BusinessValidations(item);

            if (validationErrors.Any()) throw new BusinessException(validationErrors.ToList());
        }

        private DbSet<TEntity> GetBaseDataSet()
        {
            var info = dataContext.GetType().GetProperty(typeof(TEntity).Name);

            return (DbSet<TEntity>)info.GetValue(dataContext);
        }
    }
}