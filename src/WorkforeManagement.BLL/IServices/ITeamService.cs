using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface ITeamService
    {
        Task<bool> CreateTeamAsync(string title, string description, string teamLeaderId, User currentUser);
        Task<bool> EditTeamAsync(string id, string title, string description);
        Task DeleteTeamAsync(string id);
        Task AssignUserToTeamAsync(string userId, string teamId);
        Task UnassignUserFromTeamAsync(string userId, string teamId);
        Task<Team> GetTeamByIdAsync(string id);
        Team GetTeamByTitleAsync(string title);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<User>> GetAllMembersOfTeam(string id);
        Task<User> GetTeamLeadOfTeamAsync(string id);
        Task<bool> CheckUserAccessAsync(User user, string teamId);
    }
}