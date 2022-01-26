using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.BLL.UnitTests;
using Xunit;

namespace WorkforceManagement.BLL.UnitTests
{
    public class TeamServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateTeam_Successfully_Creates_With_Correct_Data()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);

            await sut.CreateTeamAsync(new Team());

            TeamRepository.Verify(mock => mock.CreateAsync(It.IsAny<Team>()), Times.Once);
        }

        [Fact]
        public async Task EditTeam_Successfully_Edits_With_Correct_Data()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);
            var team = new Team();

            TeamRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(team);

            await sut.EditTeamAsync(team);

            TeamRepository.Verify(mock => mock.EditAsync(team), Times.Once);
        }

        [Fact]
        public async Task DeleteTeam_Successfully_Deletes_If_Team_Exists()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);
            var team = new Team();

            TeamRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(team);

            await sut.DeleteTeamAsync(team.Id.ToString());

            TeamRepository.Verify(mock => mock.DeleteAsync(team), Times.Once);
        }

        [Fact]
        public async Task DeleteTeam_Fails_If_Team_DoNot_Exist()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);
            var team1 = new Team();
            var team2 = new Team();

            TeamRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(team1);

            await sut.DeleteTeamAsync(team2.Id.ToString());

            TeamRepository.Verify(mock => mock.DeleteAsync(team2), Times.Never);
        }

        [Fact]
        public async Task GetTeamById_Successfully_Returns_Existing_Team()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);
            var team = new Team();

            TeamRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(team);

            await sut.GetTeamByIdAsync(team.Id.ToString());

            TeamRepository.Verify(mock => mock.FindByIdAsync(team.Id.ToString()), Times.Once);
        }

        [Fact]
        public async Task GetAllTeams_Successfully_Returns_Collection()
        {
            var sut = new TeamService(TeamRepository.Object, UserManager.Object);
            var teams = new List<Team>();
            teams.Add(new Team());
            teams.Add(new Team());

            TeamRepository.Setup(tr => tr.GetAllAsync()).ReturnsAsync(teams);

            var result = await sut.GetAllTeamsAsync();

            Assert.Equal(result, teams);
        }
    }
}
