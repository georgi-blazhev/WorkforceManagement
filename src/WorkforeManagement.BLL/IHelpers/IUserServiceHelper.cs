using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IHelpers
{
    public interface IUserServiceHelper
    {
        string FormatRole(string role);
        Task CheckDuplicateEmailAndUsernameAsync(User model);
        Task CheckIfUserIsTeamLeaderAsync(User user);
    }
}
