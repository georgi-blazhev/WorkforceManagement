using Xunit;
using Moq;
using WorkforceManagement.WEB.Controllers;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.Team;
using WorkforceManagement.Models.User;

namespace WorkforceManagement.WEB.UnitTests
{
    public class TeamsControllerTests : BaseWebTest
    {
        [Fact]
        public async void Get_Calls_GetTeamByIdAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.Get(It.IsAny<string>());

            TeamService.Verify(mock => mock.GetTeamByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetAll_Calls_GetAllTeamsAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.GetAll();

            TeamService.Verify(mock => mock.GetAllTeamsAsync(), Times.Once);
        }

        [Fact]
        public async void Create_Calls_CreateTeamAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            var team = new Team();
            var model = new CreateTeamModel() { Title = "fake-title" };

            Mapper.Setup(m => m.Map<Team>(model)).Returns(team);
            TeamService.Setup(ts => ts.CreateTeamAsync(It.IsAny<Team>())).ReturnsAsync(team);

            await sut.Create(model);

            TeamService.Verify(mock => mock.CreateTeamAsync(team), Times.Once);
        }

        [Fact]
        public async void Edit_Calls_EditTeamAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            var team = new Team();
            var model = new EditTeamModel() { Title = "fake-title" };

            Mapper.Setup(m => m.Map<Team>(model)).Returns(team);
            TeamService.Setup(ts => ts.EditTeamAsync(It.IsAny<Team>()));

            await sut.Edit(team.Id.ToString(), model);

            TeamService.Verify(mock => mock.EditTeamAsync(team), Times.Once);
        }

        [Fact]
        public async void Delete_Calls_DeleteTeamAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.Delete(It.IsAny<string>());

            TeamService.Verify(mock => mock.DeleteTeamAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void AssignUserToTeam_Calls_AssignUserToTeamAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            var user = new User();
            var team = new Team();
            var userModel = new CreateUserModel() { };
            var teamModel = new CreateTeamModel() { };

            Mapper.Setup(m => m.Map<User>(userModel)).Returns(user);
            Mapper.Setup(m => m.Map<Team>(teamModel)).Returns(team);

            await sut.AssignUserToTeam(user.Id, team.Id.ToString());

            TeamService.Verify(mock => mock.AssignUserToTeamAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void UnassignUserToTeam_Calls_UnassignUserToTeamAsync()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            var user = new User();
            var team = new Team();
            var userModel = new CreateUserModel() { };
            var teamModel = new CreateTeamModel() { };

            Mapper.Setup(m => m.Map<User>(userModel)).Returns(user);
            Mapper.Setup(m => m.Map<Team>(teamModel)).Returns(team);

            await sut.UnassignUserFromTeam(user.Id, team.Id.ToString());

            TeamService.Verify(mock => mock.UnassignUserFromTeamAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void SetTeamLeader_Calls_SetTeamLeader()
        {
            var sut = new TeamsController(TeamService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            var user = new User();
            var team = new Team();
            var userModel = new CreateUserModel() { };
            var teamModel = new CreateTeamModel() { };

            Mapper.Setup(m => m.Map<User>(userModel)).Returns(user);
            Mapper.Setup(m => m.Map<Team>(teamModel)).Returns(team);

            await sut.SetTeamLeader(user.Id, team.Id.ToString());

            TeamService.Verify(mock => mock.SetTeamLeader(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}