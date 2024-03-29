﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Services
{
    public class WorkforceUserManager : UserManager<User>, IUserManager
    {
        public WorkforceUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger) :
            base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
        {

        }


        public async Task<List<User>> GetAllAsync()
        {
            return await Users.ToListAsync();
        }
        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await Users.FirstOrDefaultAsync(u => u.UserName == principal.Identities.ToList()[0].Name);            
        }
        public async Task<User> CreateUserAsync(User user, string password)
        {
            await CreateAsync(user, password);
            return await FindByNameAsync(user.UserName);
        }
        public async Task UpdateUserAsync(User user, string currentPassword, string newPassword)
        {
            await UpdateAsync(user);
            await ChangePasswordAsync(user, currentPassword, newPassword);
        }
        public async Task DeleteUserAsync(User user)
        {
            await DeleteAsync(user);
        }
        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }
        public async Task<bool> ValidateUserCredentialsAsync(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }
    }
}
