using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.ViewModels;
using Sat.Recruitment.Business.Interfaces;
using Sat.Recruitment.Domain.Enumerartions;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        private Mock<IUserService> mockUserService;

        [Fact]
        public void Test1()
        {
            mockUserService = new Mock<IUserService>();

            var userController = new UsersController(mockUserService.Object);

            var result = userController.CreateUser(new UserVM("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", UserType.Normal, 100)).Result;

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Messages);
        }

        [Fact]
        public void Test2()
        {
            mockUserService = new Mock<IUserService>();

            var userController = new UsersController(mockUserService.Object);

            var result = userController.CreateUser(new UserVM("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", UserType.Normal, 200)).Result;

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Messages);
        }
    }
}