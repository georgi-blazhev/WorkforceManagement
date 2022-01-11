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
            var allTeams = await _teamService.GetAllTeamsAsync();
            List<TeamResponseModel> teamModels = new();

            foreach (var team in allTeams)
            {
                teamModels.Add(new TeamResponseModel()
                {
                    Id = team.Id,
                    Title = team.Title,
                    Description = team.Description,
                });
            }

            return teamModels;
        }

        [HttpGet]
        [Route("{teamId}/Members")]
        public async Task<List<UserResponseModel>> GetMembers(string teamId)
        {
            var allUsers = await _teamService.GetAllMembersOfTeam(teamId);
            List<UserResponseModel> members = new();

            foreach (var user in allUsers)
            {
                members.Add(new UserResponseModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }

            return members;
        }

        [HttpPost]
        public void Create(CreateTeamModel team)
        {
            if (ModelState.IsValid)
            {
                _teamService.CreateTeamAsync(team.Title, team.Description);
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
        public void AssignUserToTeam(string userId, string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.AssignUserToTeam(userId, teamId);
            }
        }

        [HttpDelete]
        [Route("{teamId}/Unassign/{userId}")]
        public void UnassignUserFromTeam(string userId, string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.UnassignUserFromTeam(userId, teamId);
            }
        }
    }
}
