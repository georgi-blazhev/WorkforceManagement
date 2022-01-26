using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByNameAsync(string name);
        Task<User> GetUserByEmailAsync(string eMail);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<User> CreateUserAsync(User newUser, string passWord, string role);
        Task EditUserAsync(User userToBeEdited, string currentPassword, string newPassword);
        Task DeleteUserAsync(string userId);
    }
}
    