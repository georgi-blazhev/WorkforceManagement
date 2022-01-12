using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserManager _userManager;

        public TeamService(ITeamRepository teamRepository, IUserManager userManager)
        {
            _teamRepository = teamRepository;
            _userManager = userManager;
        }

        public async Task<bool> CreateTeamAsync(string title, string description)
        {
            var team =  _teamRepository.FindAsync(t => t.Title == title).Result.FirstOrDefault();
            if (team != null) return false;

            await _teamRepository.CreateAsync(new Team() { Title = title, Description = description });
            return true;
        }

        public async Task<bool> EditTeamAsync(string id, string title, string description)
        {
            var teamToEdit = _teamRepository.FindAsync(t => t.Title == title).Result.FirstOrDefault();
            if (teamToEdit != null) return false;

            teamToEdit = await _teamRepository.FindByIdAsync(id);
            teamToEdit.Title = title;
            teamToEdit.Description = description;

            _teamRepository.Edit(teamToEdit);
            return true;
        }

        public async Task DeleteTeamAsync(string id)
        {
            var team = await _teamRepository.FindByIdAsync(id);
            _teamRepository.Delete(team);
        }

        public async Task AssignUserToTeamAsync(string userId, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);

            team.Members.Add(user);
        }

        public async Task UnassignUserFromTeamAsync(string userId, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);

            team.Members.Remove(user);
        }

        public async Task<Team> GetTeamByIdAsync(string id)
        {
            return await _teamRepository.FindByIdAsync(id);
        }

        public Team GetTeamByTitleAsync(string title)
        {
            return _teamRepository.FindAsync(t => t.Title == title).Result.FirstOrDefault();
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