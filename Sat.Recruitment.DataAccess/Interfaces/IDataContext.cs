using System;
using System.Data.Entity;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.DataAccess.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();

        #region Entities

        IDbSet<User> User { get; set; }

        #endregion
    }
}