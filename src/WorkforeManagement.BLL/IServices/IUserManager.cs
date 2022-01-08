using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IServices
{
    public interface IUserManager
    {
        Task CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<User> FindByUserNameAsync(string userName);
        Task<User> FindByIdAsync(string id);
        Task<User> UpdateUserAsync(User user, string currentPassword, string newPassword);
        Task<List<User>> GetAllAsync();
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<bool> ValidateUserCredentials(string userName, string password);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        public void AssignUserToTeam(User user, Team team);
        public void UnassignUserToTeam(User user, Team team);
    }
}
