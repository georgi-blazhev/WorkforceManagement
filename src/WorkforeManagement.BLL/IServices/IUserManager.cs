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
        Task<User> FindByIdAsync(string id);
        Task<User> FindByUserNameAsync(string userName);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<List<User>> GetAllAsync();
        Task<List<string>> GetUserRolesAsync(User user);
        Task CreateUserAsync(User user, string password);
        Task<User> UpdateUserAsync(User user, string currentPassword, string newPassword);
        Task DeleteUserAsync(User user);
        Task<bool> ValidateUserCredentials(string userName, string password);
        public void AssignUserToTeam(User user, Team team);
        public void UnassignUserToTeam(User user, Team team);
    }
}
