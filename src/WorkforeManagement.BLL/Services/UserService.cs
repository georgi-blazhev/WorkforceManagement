using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IHelpers;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.Repositories;

namespace WorkforceManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        private readonly ITimeOffRequestRepository _timeOffRequestRepository;
        private readonly IUserServiceHelper _helper;

        public UserService(IUserManager userManager, ITimeOffRequestRepository timeOffRequestRepository, IUserServiceHelper helper)
        {
            _userManager = userManager;
            _timeOffRequestRepository = timeOffRequestRepository;
            _helper = helper;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<User> GetUserByEmailAsync(string eMail)
        {
            return await _userManager.FindByEmailAsync(eMail);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.GetAllAsync();
        }
        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetCurrentUser(principal);
        }
        public async Task<User> CreateUserAsync(User newUser, string passWord, string role)
        {
            await _helper.CheckDuplicateEmailAndUsernameAsync(newUser);

            newUser.CreatedAt = DateTime.Now;
            role = _helper.FormatRole(role);

            newUser = await _userManager.CreateUserAsync(newUser, passWord);
            await _userManager.AddToRoleAsync(newUser, role);

            return newUser;
        }
        public async Task EditUserAsync(User modelWithUpdates, string currentPassword, string newPassword)
        {
            await _helper.CheckDuplicateEmailAndUsernameAsync(modelWithUpdates);

            var userToBeEdited = await _userManager.FindByIdAsync(modelWithUpdates.Id);

            userToBeEdited.UserName = modelWithUpdates.UserName;
            userToBeEdited.FirstName = modelWithUpdates.FirstName;
            userToBeEdited.LastName = modelWithUpdates.LastName;
            userToBeEdited.Email = modelWithUpdates.Email;

            await _userManager.UpdateUserAsync(userToBeEdited, currentPassword, newPassword);
        }
        public async Task DeleteUserAsync(string userId)
        {
            var userToBeDeleted = await _userManager.FindByIdAsync(userId);
            if (_userManager.GetUserRolesAsync(userToBeDeleted).Result.Contains("Admin"))
                throw new ArgumentException("You can not delete Admin users! ");

            var userTimeOffs = await _timeOffRequestRepository.GetAllTimeOffsByUser(userToBeDeleted);
            await _timeOffRequestRepository.DeleteCollectionAsync(userTimeOffs.ToList());

            await _helper.CheckIfUserIsTeamLeaderAsync(userToBeDeleted);

            await _userManager.DeleteUserAsync(userToBeDeleted);
        }        
    }
}
