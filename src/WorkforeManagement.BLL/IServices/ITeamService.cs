using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface ITeamService
    {
        Task<Team> GetTeamByIdAsync(string teamId);
        Task<List<Team>> GetAllTeamsAsync();
        Task<Team> CreateTeamAsync(Team newTeam);
        Task EditTeamAsync(Team modelWithUpdates);
        Task DeleteTeamAsync(string teamId);
        Task AssignUserToTeamAsync(string userId, string teamId);
        Task UnassignUserFromTeamAsync(string userId, string teamId);
        Task SetTeamLeader(string userId, string teamId);
    }
}