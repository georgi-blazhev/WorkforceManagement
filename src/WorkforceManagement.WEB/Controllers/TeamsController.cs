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
        public async Task<IActionResult> Create(CreateTeamModel team)
        {
            bool teamWasCreated = await _teamService.CreateTeamAsync(team.Title, team.Description);
            if (teamWasCreated) return Ok("Team was successfully created! ");
            return BadRequest("A Team with such Title already exsists! ");
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> Edit(string teamId, EditTeamModel team)
        {
            bool teamWasEdited = await _teamService.EditTeamAsync(teamId, team.Title, team.Description);
            if (teamWasEdited) return Ok("Team was successfully edited! ");
            return BadRequest("A Team with such Title already exsists! ");
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete(string teamId)
        {
            await _teamService.DeleteTeamAsync(teamId);
            return Ok("Team was successfully deleted! ");
        }

        [HttpPost]
        [Route("{teamId}/Assign/{userId}")]
        public async Task<IActionResult> AssignUserToTeam(string userId, string teamId)
        {
            await _teamService.AssignUserToTeamAsync(userId, teamId);
            return Ok("User was successfully added to the Team! ");
        }

        [HttpDelete]
        [Route("{teamId}/Unassign/{userId}")]
        public async Task<IActionResult> UnassignUserFromTeam(string userId, string teamId)
        {
            await _teamService.UnassignUserFromTeamAsync(userId, teamId);
            return Ok("User was successfully removed from the Team! ");
        }
    }
}
