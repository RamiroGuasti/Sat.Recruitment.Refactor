using Common.Net.Utils;
using Sat.Recruitment.Domain.Entities.Base;

namespace Sat.Recruitment.Api.ViewModels
{
    public class BaseVM<TEntity> where TEntity : BaseEntity, new()
    {
        public int Id { get; set; }

        public BaseVM()
        {
        }

        public BaseVM(TEntity baseEntity)
        {
            FromEntityModel(baseEntity);
        }

        public virtual TEntity ToEntityModel()
        {
            var item = new TEntity();

            item.MapFromReflectedProperties(this);
            
            return item;
        }

        public virtual void FromEntityModel(TEntity item)
        {
            this.MapFromReflectedProperties(item);
        }
    }
}