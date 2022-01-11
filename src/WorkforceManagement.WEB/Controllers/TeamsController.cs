using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.Models.DTO.Requests.TeamRequests;
using WorkforceManagement.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagment.Models.DTO.Responses;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService) : base()
        {
            _teamService = teamService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<List<TeamResponseModel>> GetAll()
        {
            var teams = new List<TeamResponseModel>();
            var teamsFromDb = await _teamService.GetAllTeamsAsync();

            foreach (var team in teamsFromDb)
            {
                teams.Add(new TeamResponseModel()
                {
                    Title = team.Title,
                    Description = team.Description,
                });
            }

            return teams;
        }

        [HttpGet]
        [Route("{teamId}/Members")]
        public async Task<List<UserResponseModel>> GetMembers(string teamId)
        {
            var users = new List<UserResponseModel>();
            var usersFromDb = await _teamService.GetAllMembersOfTeam(teamId);

            foreach (var user in usersFromDb)
            {
                users.Add(new UserResponseModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }

            return users;
        }

        [HttpPost]
        public void Create(CreateTeamModel team) // TODO: Do we need a whole model just to pass a single property?
        {
            if (ModelState.IsValid)
            {
                _teamService.CreateTeam(team.Title);
            }
        }

        [HttpPut("{teamId}")]
        public void Edit(string teamId, EditTeamModel team)
        {
            if (ModelState.IsValid)
            {
                _teamService.EditTeam(teamId, team.Title, team.Description);
            }
        }

        [HttpDelete("{teamId}")]
        public void Delete(string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.DeleteTeam(teamId);
            }
        }

        [HttpPost]
        [Route("{teamId}/Assign/{userId}")]
        public void AssignUserToTeam(User user, string teamId) // TODO: How are you passing a whole user?
        {
            if (ModelState.IsValid)
            {
                _teamService.AssignUserToTeam(user, teamId);
            }
        }

        [HttpDelete]
        [Route("{teamId}/Unassign/{userId}")]
        public void DeleteUserFromTeam(User user, string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.DeleteUserFromTeam(user, teamId);
            }
        }
    }
}
