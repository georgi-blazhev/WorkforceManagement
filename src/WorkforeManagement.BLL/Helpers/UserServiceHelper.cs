using System;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IHelpers;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Helpers
{
    public class UserServiceHelper : IUserServiceHelper
    {
        private readonly IUserManager _userManager;
        private readonly ITeamService _teamService;
        public UserServiceHelper(IUserManager userManager, ITeamService teamService)
        {
            _userManager = userManager;
            _teamService = teamService;
        }
        public string FormatRole(string role)
        {
            role = role.ToUpper().Trim();
            if (role.Equals(Role.Admin.ToString().ToUpper()))
            {
                return "Admin";
            }
            else if (role.Equals(Role.Regular.ToString().ToUpper()))
            {
                return "Regular";
            }
            throw new ArgumentException("The Role can only be Admin or Regular! ");
        }
        public async Task CheckDuplicateEmailAndUsernameAsync(User model)
        {
            var userWithSameName = await _userManager.FindByNameAsync(model.UserName);
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userWithSameName != null && userWithSameName.Id != model.Id)
                throw new ArgumentException("A user with such Username already exists! ");

            if (userWithSameEmail != null && userWithSameEmail.Id != model.Id)
                throw new ArgumentException("A user with such Email already exists! ");
        }
        public async Task CheckIfUserIsTeamLeaderAsync(User user)
        {
            var allTeams = await _teamService.GetAllTeamsAsync();
            if (allTeams.Any(t => t.TeamLeader.Id == user.Id))
                throw new InvalidOperationException($"Assign a new Team Leader before deleting! ");
        }
    }
}
