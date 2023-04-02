using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Net.Utils;
using Common.Net.Utils.CustomException;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.ViewModels;
using Sat.Recruitment.Business.Interfaces;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Enumerartions;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService; 
        }

        [HttpGet]
        [Route("/list-users")]
        public IEnumerable<User> Get()
        {
            return userService.GetUsers();
        }

        [HttpGet]
        [Route("/list-users2")]
        public IEnumerable<User> Get2()
        {
            return GetUsers();
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(UserVM userVM)
        {
            try
            {
                var response = await userService.CreateUser(userVM.ToEntityModel());

                return new Result(true, response.ToString());
            }
            
            // NOTE: error validation has must be implemented in a custom ExceptionFilterAttribute (logging the error!)
            catch (BusinessException be)
            {
                return new Result(false, string.Join("\n", be.Errors.Select(x => x.Description).ToList()));
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        private List<User> GetUsers()
        {
            var users = new List<User>();

            using (var reader = ReadUsersFromFile())
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLineAsync().Result;

                    var user = new User(line.Split(',')[0], // name
                                        line.Split(',')[1], // email
                                        line.Split(',')[2], // address
                                        line.Split(',')[3], // phone
                                        line.Split(',')[4] == EnumsUtils.GetDescriptionAttribute(UserType.Normal) ? UserType.Normal : UserType.SuperUser, // type
                                        decimal.Parse(line.Split(',')[5])); // money

                    users.Add(user);
                }
            }
            
            return users;
        }
    }

    // NOTE: this class is not required with a custom filter
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }

        public Result() { }

        public Result(bool isSuccess, string messages) : this()
        {
            IsSuccess = isSuccess;
            Messages = messages;
        }
    }
}