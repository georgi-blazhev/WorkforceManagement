using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface IUserManager
    {
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByNameAsync(string userName);
        Task<User> FindByEmailAsync(string name);
        Task<List<User>> GetAllAsync();
        Task<User> GetCurrentUser(ClaimsPrincipal principal);
        Task<User> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user, string currentPassword, string newPassword);
        Task DeleteUserAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> ValidateUserCredentialsAsync(string userName, string password);
    }
}
