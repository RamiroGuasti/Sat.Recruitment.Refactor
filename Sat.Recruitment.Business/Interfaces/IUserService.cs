using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Business.Interfaces.Base;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Business.Interfaces
{
    public interface IUserService : IBaseService
    {
        IEnumerable<User> GetUsers();

        Task<int> CreateUser(User user);
    }
}