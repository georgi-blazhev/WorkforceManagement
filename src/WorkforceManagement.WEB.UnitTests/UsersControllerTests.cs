using Moq;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.User;
using WorkforceManagement.WEB.Controllers;
using Xunit;

namespace WorkforceManagement.WEB.UnitTests
{
    public class UsersControllerTests : BaseWebTest
    {
        [Fact]
        public async void Get_Default_CallsGetUserByIdAsync()
        {
            // arrange
            var sut = new UsersController(UserService.Object, Mapper.Object, LinkGenerator.Object);

            // act
            await sut.Get("fake-id");

            // assert
            UserService.Verify(mock =>
                mock.GetUserByIdAsync("fake-id"),
                Times.Once);
        }

        [Fact]
        public async void AllUsers_Default_CallsGetAllUsersAsync()
        {
            // arrange
            var sut = new UsersController(UserService.Object, Mapper.Object, LinkGenerator.Object);

            // act
            await sut.AllUsers();

            // assert
            UserService.Verify(mock =>
                mock.GetAllUsersAsync(),
                Times.Once);
        }

        [Fact]
        public async void Create_Default_CallsCreateUserAsync()
        {
            // arrange
            var user = new User() { Id = "fake-id"};
            var model = new CreateUserModel() { Password = "fake-password" , Role = "fake-role"};
            var sut = new UsersController(UserService.Object, Mapper.Object, LinkGenerator.Object);
            Mapper.Setup(map => map.Map<User>(model)).Returns(user);
            UserService.Setup(us => us.CreateUserAsync(user, model.Password, model.Role)).ReturnsAsync(user);

            // act
            await sut.Create(model);

            // assert
            UserService.Verify(mock =>
                mock.CreateUserAsync(user, model.Password, model.Role),
                Times.Once);
        }

        [Fact]
        public async void Edit_Default_CallsEditUserAsync()
        {
            // arrange
            var user = new User() { Id = "fake-id" };
            var model = new EditUserModel() { CurrentPassword = "fake-curr-pass", NewPassword = "fake-new-pass"};
            var sut = new UsersController(UserService.Object, Mapper.Object, LinkGenerator.Object);
            Mapper.Setup(map => map.Map<User>(model)).Returns(user);
            UserService.Setup(us => us.EditUserAsync(user, model.CurrentPassword, model.NewPassword));

            // act
            await sut.Edit(model, "fake-id");

            // assert
            UserService.Verify(mock =>
                mock.EditUserAsync(user, model.CurrentPassword, model.NewPassword),
                Times.Once);
        }

        [Fact]
        public async void Delete_Default_CallsDeleteUserAsync()
        {
            // arrange
            var sut = new UsersController(UserService.Object, Mapper.Object, LinkGenerator.Object);

            // act
            await sut.Delete("fake-id");

            // assert
            UserService.Verify(mock =>
                mock.DeleteUserAsync("fake-id"),
                Times.Once);
        }

    }
}
