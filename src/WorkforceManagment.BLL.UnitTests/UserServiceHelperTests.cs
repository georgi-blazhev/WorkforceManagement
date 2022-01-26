using Moq;
using System;
using System.Collections.Generic;
using WorkforceManagement.BLL.Helpers;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.BLL.UnitTests;
using Xunit;

namespace WorkforceManagement.BLL.UnitTests
{
    public class UserServiceHelperTests : BaseServiceTest
    {
        [Fact]
        public void FormatRole_Admin_ReturnsAdminRole()
        {
            // arrange
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act
            var result = sut.FormatRole("admin");

            // assert
            Assert.Equal(Role.Admin.ToString(), result);
        }

        [Fact]
        public void FormatRole_Regular_ReturnsRegularRole()
        {
            // arrange
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act
            var result = sut.FormatRole("regular");

            // assert
            Assert.Equal(Role.Regular.ToString(), result);
        }

        [Fact]
        public void FormatRole_NotAdminOrRegular_ThrowsArgumentException()
        {
            // arrange
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act & assert
            Assert.Throws<ArgumentException>(() => sut.FormatRole("random-string"));
        }

        [Fact]
        public void CheckIfUserIsTeamLeader_TeamLeader_ThrowsArgumentException()
        {
            // arrange
            var teamLeader = new User() { Id = "fake-id" };
            var team = new Team() { TeamLeader = teamLeader };
            TeamService.Setup(ts => ts.GetAllTeamsAsync()).ReturnsAsync(new List<Team>() { team });
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act & assert
            Assert.ThrowsAsync<ArgumentException>(async () => await sut.CheckIfUserIsTeamLeaderAsync(teamLeader));
        }

        [Fact]
        public async void CheckIfUserIsTeamLeader_NotTeamLeader_DoesNotThrowArgumentException()
        {
            // arrange
            var teamMember = new User();
            var team = new Team() { TeamLeader = teamMember };
            TeamService.Setup(ts => ts.GetAllTeamsAsync()).ReturnsAsync(new List<Team>());
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            //Act
            var exception = await Record.ExceptionAsync(async () => await sut.CheckIfUserIsTeamLeaderAsync(teamMember));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void CheckDuplicateEmailAndUsername_DuplicateUsername_ThrowsArgumentException()
        {
            // arrange
            var model = new User() { Id = "fake-id", UserName = "fake-username", Email = "fake-email" };
            UserManager.Setup(man => man.FindByNameAsync(model.UserName)).ReturnsAsync(new User());
            UserManager.Setup(man => man.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.CheckDuplicateEmailAndUsernameAsync(model));
        }

        [Fact]
        public async void CheckDuplicateEmailAndUsername_DuplicateEmail_ThrowsArgumentException()
        {
            // arrange
            var model = new User() { Id = "fake-id", UserName = "fake-username", Email = "fake-email" };
            UserManager.Setup(man => man.FindByNameAsync(model.UserName)).ReturnsAsync((User)null);
            UserManager.Setup(man => man.FindByEmailAsync(model.Email)).ReturnsAsync(new User());
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.CheckDuplicateEmailAndUsernameAsync(model));
        }

        [Fact]
        public async void CheckDuplicateEmailAndUsername_ValidUsernmeAndEmail_DoesNotThrowArgumentException()
        {
            // arrange
            var model = new User() { Id = "fake-id", UserName = "fake-username", Email = "fake-email" };
            UserManager.Setup(man => man.FindByNameAsync(model.UserName)).ReturnsAsync((User)null);
            UserManager.Setup(man => man.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);
            var sut = new UserServiceHelper(UserManager.Object, TeamService.Object);

            //Act
            var exception = await Record.ExceptionAsync(async () => await sut.CheckDuplicateEmailAndUsernameAsync(model));

            //Assert
            Assert.Null(exception);
        }
    }
}
