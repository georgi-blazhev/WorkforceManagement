using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IUserManager _userManager;

        public TeamService(IRepository<Team> teamRepository, IUserManager userManager)
        {
            _teamRepository = teamRepository;
            _userManager = userManager;
        }

        public async Task CreateTeam(string title)
        {
            var team =  _teamRepository.FindByNameAsync(title);
            if (team != null)
            {
                throw new DuplicateNameException("A team with this title already exist");
            }

            await _teamRepository.CreateAsync(new Team() { Title = title });
        }

        public async Task EditTeam(string id, string title, string description)
        {
            var teamToEdit = await _teamRepository.FindByIdAsync(id);
            teamToEdit.Title = title;
            teamToEdit.Description = description;

            _teamRepository.Edit(teamToEdit);
        }

        public async Task DeleteTeam(string id)
        {
            var team = await _teamRepository.FindByIdAsync(id);
            _teamRepository.Delete(team);
        }

        public async Task AssignUserToTeam(User user, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);

            team.Members.Add(user);
        }

        public async Task DeleteUserFromTeam(User user, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);

            team.Members.Remove(user);
        }

        public async Task<Team> GetTeamByIdAsync(string id)
        {
            return await _teamRepository.FindByIdAsync(id);
        }

        public async Task<Team> GetTeamByTitleAsync(string title)
        {
            return await _teamRepository.FindByNameAsync(title);
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<List<User>> GetAllMembersOfTeam(string id)
        {
            Team team = await _teamRepository.FindByIdAsync(id);

            return team.Members;
        }

        public async Task<User> GetTeamLeadOfTeamAsync(string id)
        {
            Team team = await _teamRepository.FindByIdAsync(id);

            return team.TeamLeader;
        }

        public async Task<bool> CheckUserAccessAsync(User user, string teamId)
        {
            var currentUserRoles = await _userManager.GetUserRolesAsync(user);
            var team = await _teamRepository.FindByIdAsync(teamId);

            if (team.Members.Contains(user) || currentUserRoles.Contains("admin"))
            {
                return true;
            }

            return false;
        }
    }
}