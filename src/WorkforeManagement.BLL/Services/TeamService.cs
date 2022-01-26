using System;
using System.Collections.Generic;
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

        public async Task<Team> GetTeamByIdAsync(string teamId)
        {
            return await _teamRepository.FindByIdAsync(teamId);
        }
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }
        public async Task<Team> CreateTeamAsync(Team newTeam)
        {
            var team = _teamRepository.FindAsync(t => t.Title == newTeam.Title).Result.FirstOrDefault();
            if (team != null) 
                throw new ArgumentException("A team with such Title already exists! ");

            var teamLeader = await _userManager.FindByIdAsync(newTeam.TeamLeaderId);
            if (_teamRepository.FindAsync(t => t.TeamLeaderId == teamLeader.Id).Result.Any())
                throw new ArgumentException("This user is already a Team Leader of another team");

            newTeam.CreatedAt = DateTime.Now;
            newTeam.LastChange = DateTime.Now;
            newTeam.TeamLeader = teamLeader;
            newTeam.Members = new List<User> { teamLeader };

            await _teamRepository.CreateAsync(newTeam);
            return _teamRepository.FindAsync(t => t.Title == newTeam.Title).Result.FirstOrDefault();
        }
        public async Task EditTeamAsync(Team teamWithUpdates)
        {
            var teamToEdit = _teamRepository.FindAsync(t => t.Title == teamWithUpdates.Title).Result.FirstOrDefault();
            if (teamToEdit != null && teamToEdit.Id != teamWithUpdates.Id)
                throw new ArgumentException("A team with such Title already exists! ");

            teamToEdit = await _teamRepository.FindByIdAsync(teamWithUpdates.Id.ToString());
            
            teamToEdit.Title = teamWithUpdates.Title;
            teamToEdit.Description = teamWithUpdates.Description;
            teamToEdit.LastChange = DateTime.Now;

            await _teamRepository.EditAsync(teamToEdit);
        }
        public async Task DeleteTeamAsync(string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            await _teamRepository.DeleteAsync(team);
        }
        public async Task AssignUserToTeamAsync(string userId, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);
            if (team.Members.Contains(user))
                throw new ArgumentException("This user is already a part of this team");

            team.LastChange = DateTime.Now;

            await _teamRepository.AssignUserToTeamAsync(user, team);
        }
        public async Task UnassignUserFromTeamAsync(string userId, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);
            if (!team.Members.Contains(user))
                throw new ArgumentException("This user is not a part of this team");

            if (team.TeamLeaderId == user.Id)
                throw new ArgumentException("This user is the Team Lead of this team");

            team.LastChange = DateTime.Now;

            await _teamRepository.UnssignUserToTeamAsync(user, team);
        }
        public async Task SetTeamLeader(string userId, string teamId)
        {
            var team = await _teamRepository.FindByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);
            if (_teamRepository.FindAsync(t => t.TeamLeaderId == user.Id).Result.Any())
                throw new ArgumentException("This user is already a Team Leader of another team");

            team.Members.Add(user);
            team.LastChange = DateTime.Now;

            await _teamRepository.AssignNewTeamLead(user, team);
        }
    }
}