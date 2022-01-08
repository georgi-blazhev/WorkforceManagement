using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        public UserService(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.GetAllAsync();
        }
        public async Task<List<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetUserRolesAsync(user);
        }
        public async Task<bool> CreateUserAsync(string userName, string passWord, string firstName, string lastName, Role role, string teamId)
        {
            if (await _userManager.FindByNameAsync(userName) != null) return false;

            var team = _uow.TeamRepository.FindById(teamId);

            User newUser = new()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName
            };

            await _userManager.CreateUserAsync(newUser, passWord);
            newUser.Teams.Add(team);
            _uow.SaveChanges();

            AddUserToRoleAsync(newUser, role);
            return true;
        }
        public async Task<User> EditUserAsync(string userId, string userName, string currentPassword, string newPassword, string firstName, string lastName)
        {
            User userToBeEdited = await _userManager.FindByIdAsync(userId);

            userToBeEdited.UserName = userName;
            userToBeEdited.FirstName = firstName;
            userToBeEdited.LastName = lastName;

            return await _userManager.UpdateUserAsync(userToBeEdited, currentPassword, newPassword);
        }
        public async System.Threading.Tasks.Task DeleteUserAsync(string userId)
        {
            var userToBeDeleted = await _userManager.FindByIdAsync(userId);

            _uow.WorkLogRepository.DeleteCollection(userToBeDeleted.WorkLogs);
            _uow.SaveChanges();
            await _userManager.DeleteUserAsync(userToBeDeleted);
        }
        public async Task<bool> AssignUserToTeamAsync(string userId, string teamId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            Team team = _uow.TeamRepository.FindById(teamId);

            _userManager.AssignUserToTeam(user, team);
            _uow.SaveChanges();
            return true;
        }
        public async Task<bool> UnassignUserToTeamAsync(string userId, string teamId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            Team team = _uow.TeamRepository.FindById(teamId);

            _userManager.UnassignUserToTeam(user, team);
            _uow.SaveChanges();
            return true;
        }

        public async void AddUserToRoleAsync(User user, Role role)
        {
            if (role == Role.Admin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else if (role == Role.Manager)
            {
                await _userManager.AddToRoleAsync(user, "Manager");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Regular");
            }
        }
    }
}
