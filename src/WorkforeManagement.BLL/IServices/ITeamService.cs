using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface ITeamService
    {
        Task CreateTeamAsync(string title, string description);
        Task EditTeam(string id, string title, string description);
        Task DeleteTeam(string id);
        Task AssignUserToTeam(string userId, string teamId);
        Task UnassignUserFromTeam(string userId, string teamId);
        Task<Team> GetTeamByIdAsync(string id);
        Team GetTeamByTitleAsync(string title);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<User>> GetAllMembersOfTeam(string id);
        Task<User> GetTeamLeadOfTeamAsync(string id);
        Task<bool> CheckUserAccessAsync(User user, string teamId);
    }
}