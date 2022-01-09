using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Task<List<string>> GetRolesAsync(User user);
        Task<bool> CreateUserAsync(string userName, string eMail, string passWord, string firstName, string lastName, Role role);
        Task<bool> EditUserAsync(string userId, string userName, string eMail, string currentPassword, string newPassword, string firstName, string lastName);
        Task DeleteUserAsync(string userId);
    }
}
