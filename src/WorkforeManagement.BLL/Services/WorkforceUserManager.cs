using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Services
{
    public class WorkforceUserManager : UserManager<User>, IUserManager
    {
        private readonly DatabaseContext _databaseContext;

        public WorkforceUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger, DatabaseContext databaseContext) :
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
            _databaseContext = databaseContext;
        }

        public Task CreateUserAsync(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByUserNameAsync(string userName)
        {
            return await FindByNameAsync(userName);
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }

        public async Task<bool> ValidateUserCredentials(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }

        Task<IdentityResult> IUserManager.UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
