using System.Collections.Generic;
using System.Linq;
using Sat.Recruitment.Domain.Entities.Base;

namespace Sat.Recruitment.Business.Interfaces.Base
{
    public interface IBaseEntityService<TEntity> : IBaseService where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetBaseQueryableCollection(bool asNoTracking = true);

        IQueryable<TEntity> Get(IEnumerable<int> ids, bool asNoTracking = true);

        TEntity Get(int id, bool asNoTracking = true);

        int Add(TEntity item, bool saveChanges = true);

        void Update(TEntity item, bool saveChanges = true);

        void Delete(int id, bool saveChanges = true);

        void SpicySaveChanges();
    }
}