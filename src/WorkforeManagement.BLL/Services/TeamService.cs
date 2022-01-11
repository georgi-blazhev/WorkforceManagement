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

        public async Task CreateTeam(string title)
        {
            var team =  _teamRepository.FindAsync(t => t.Title == title).Result.FirstOrDefault();
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