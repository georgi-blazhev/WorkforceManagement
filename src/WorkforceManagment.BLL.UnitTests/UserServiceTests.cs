using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.BLL.UnitTests;
using Xunit;

namespace WorkforceManagement.BLL.UnitTests
{
    public class UserServiceTests : BaseServiceTest
    {
        [Fact]
        public async void CreateUserAsync_Default_CallsCheckDuplicateEmailAndUsername()
        {
            // arrange
            var user = new User() { UserName = "fake-username" };
            UserServiceHelper.Setup(us => us.FormatRole("fake-admin-role")).Returns("Admin");
            UserManager.Setup(um => um.CreateUserAsync(user, "fake-password")).ReturnsAsync(user);
            UserManager.Setup(um => um.AddToRoleAsync(user, "Admin")).ReturnsAsync(new IdentityResult());
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            var result = await sut.CreateUserAsync(user, "fake-password", "fake-admin-role");

            // assert
            UserServiceHelper.Verify(mock =>
                mock.CheckDuplicateEmailAndUsernameAsync(It.Is<User>(u => u.UserName == "fake-username")),
                Times.Once);
        }

        [Fact]
        public async void EditUserAsync_Default_CallsCheckDuplicateEmailAndUsername()
        {
            // arrange
            var model = new User()
            {
                Id = "fake-id",
                UserName = "fake-username",
                FirstName = "fake-firstname",
                LastName = "fake-lastname",
                Email = "fake-email"
            };
            UserManager.Setup(um => um.FindByIdAsync("fake-id")).ReturnsAsync(model);
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.EditUserAsync(model, "fake-current-password", "fake-new-password");

            // assert
            UserServiceHelper.Verify(mock =>
                mock.CheckDuplicateEmailAndUsernameAsync(It.Is<User>(u => u.UserName == "fake-username")),
                Times.Once);
        }

        [Fact]
        public async void DeleteUserAsync_RegularUser_CallsCheckIfUserIsTeamLeader()
        {
            // arrange
            var model = new User() { Id = "fake-id" };
            UserManager.Setup(um => um.FindByIdAsync("fake-id")).ReturnsAsync(model);
            UserManager.Setup(um => um.GetUserRolesAsync(model)).ReturnsAsync(new List<string>());
            TimeOffRequestRepository.Setup(tor => tor.GetAllTimeOffsByUser(model)).ReturnsAsync(new List<TimeOffRequest>());
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.DeleteUserAsync("fake-id");

            // assert
            UserServiceHelper.Verify(mock =>
                mock.CheckIfUserIsTeamLeaderAsync(It.Is<User>(u => u.Id == "fake-id")),
                Times.Once);
        }

        [Fact]
        public void DeleteUserAsync_AdminUser_ThrowsArgumentException()
        {
            // arrange
            var model = new User() { Id = "fake-id" };
            UserManager.Setup(um => um.FindByIdAsync("fake-id")).ReturnsAsync(model);
            UserManager.Setup(um => um.GetUserRolesAsync(model)).ReturnsAsync(new List<string>() { "Admin" });
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act & assert
            Assert.ThrowsAsync<ArgumentException>(async () => await sut.DeleteUserAsync("fake-id"));
        }

        [Fact]
        public async void GetUserByIdAsync_Default_FindByIdAsync()
        {
            // arrange
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.GetUserByIdAsync("fake-id");

            // assert
            UserManager.Verify(mock =>
                mock.FindByIdAsync("fake-id"),
                Times.Once);
        }

        [Fact]
        public async void GetUserByNameAsync_Default_CallsFindByNameAsync()
        {
            // arrange
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.GetUserByNameAsync("fake-username");

            // assert
            UserManager.Verify(mock =>
                mock.FindByNameAsync("fake-username"),
                Times.Once);
        }

        [Fact]
        public async void GetUserByEmailAsync_Default_CallsFindByEmailAsync()
        {
            // arrange
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.GetUserByEmailAsync("fake-email");

            // assert
            UserManager.Verify(mock =>
                mock.FindByEmailAsync("fake-email"),
                Times.Once);
        }

        [Fact]
        public async void GetAllUsersAsync_Default_CallsGetAllAsync()
        {
            // arrange
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.GetAllUsersAsync();

            // assert
            UserManager.Verify(mock =>
                mock.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async void GetCurrentUserAsync_Default_GetCurrentUser()
        {
            // arrange
            var principal = new ClaimsPrincipal();
            var sut = new UserService(UserManager.Object, TimeOffRequestRepository.Object, UserServiceHelper.Object);

            // act
            await sut.GetCurrentUserAsync(principal);

            // assert
            UserManager.Verify(mock =>
                mock.GetCurrentUser(principal),
                Times.Once);
        }
    }
}
