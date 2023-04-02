using Sat.Recruitment.Business.Interfaces.Base;
using Sat.Recruitment.DataAccess.Interfaces;

namespace Sat.Recruitment.Business.Services.Base
{
    public abstract class BaseService : IBaseService
	{
		protected readonly IDataContext dataContext;

		public BaseService(IDataContext dataContext)
		{
			this.dataContext = dataContext;
		}
	}
}