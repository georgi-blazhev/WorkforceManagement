using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface ITeamService
    {
        Task CreateTeam(string title);
        Task EditTeam(string id, string title, string description);
        Task DeleteTeam(string id);
        Task AssignUserToTeam(User user, string teamId);
        Task DeleteUserFromTeam(User user, string teamId);
        Task<Team> GetTeamByIdAsync(string id);
        Task<Team> GetTeamByTitleAsync(string title);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<User>> GetAllMembersOfTeam(string id);
        Task<User> GetTeamLeadOfTeamAsync(string id);
        Task<bool> CheckUserAccessAsync(User user, string teamId);
    }
}
