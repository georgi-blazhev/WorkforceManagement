﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;

        public UserService(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetCurrentUser(principal);
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
        public async Task<List<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetUserRolesAsync(user);
        }
        public async Task<bool> CreateUserAsync(string userName, string eMail, string passWord, string firstName, string lastName, Role role)
        {
            if (await _userManager.FindByNameAsync(userName) != null) return false;
            if (await _userManager.FindByEmailAsync(eMail) != null) return false;

            User newUser = new()
            {
                UserName = userName,
                Email = eMail,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now
            };

            await _userManager.CreateUserAsync(newUser, passWord);
            await AddUserToRoleAsync(newUser, role);
            return true;
        }
        public async Task<bool> EditUserAsync(string userId, string userName, string eMail, string currentPassword, string newPassword, string firstName, string lastName)
        {
            if (await _userManager.FindByNameAsync(userName) != null) return false;
            if (await _userManager.FindByEmailAsync(eMail) != null) return false;

            User userToBeEdited = await _userManager.FindByIdAsync(userId);

            userToBeEdited.UserName = userName;
            userToBeEdited.FirstName = firstName;
            userToBeEdited.LastName = lastName;
            userToBeEdited.Email = eMail;

            await _userManager.UpdateUserAsync(userToBeEdited, currentPassword, newPassword);
            return true;
        }
        public async Task DeleteUserAsync(string userId)
        {
            var userToBeDeleted = await _userManager.FindByIdAsync(userId);
            // TODO: Delete related information such as TimeOffRequests
            await _userManager.DeleteUserAsync(userToBeDeleted);
        }   

        public async Task AddUserToRoleAsync(User user, Role role)
        {
            if (role == Role.Admin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Regular");
            }
        }
    }
}
