using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagement.BLL.IServices;
using System.Threading.Tasks;
using AutoMapper;
using WorkforceManagement.Models.Team;
using WorkforceManagement.DAL.Entities;
using Microsoft.AspNetCore.Routing;
using System;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TeamsController(ITeamService teamService, IUserService userService, IMapper mapper, LinkGenerator linkGenerator)
        {
            _teamService = teamService;
            _userService = userService;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [Route("{teamId}")]
        public async Task<ActionResult<ViewTeamDetailModel>> Get(string teamId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);
            return _mapper.Map<ViewTeamDetailModel>(team);
        }

        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<ViewTeamModel[]>> GetAll()
        {
            var allTeams = await _teamService.GetAllTeamsAsync();
            return _mapper.Map<ViewTeamModel[]>(allTeams);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamModel model)
        {
            Team newTeam = _mapper.Map<Team>(model);
            newTeam.Creator = await _userService.GetCurrentUserAsync(User);
            newTeam = await _teamService.CreateTeamAsync(newTeam);

            string location = GenerateLocation(newTeam);
            if (string.IsNullOrWhiteSpace(location)) return BadRequest();

            return Created(location, _mapper.Map<ViewTeamModel>(newTeam));
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> Edit(string teamId, EditTeamModel model)
        {
            var teamWithUpdates = _mapper.Map<Team>(model);
            teamWithUpdates.Id = Guid.Parse(teamId);
            await _teamService.EditTeamAsync(teamWithUpdates);

            return Ok("The Team was successfully edited! ");
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

        [HttpPost]
        [Route("{teamId}/AssignTL/{userId}")]
        public async Task<IActionResult> SetTeamLeader(string userId, string teamId)
        {
            await _teamService.SetTeamLeader(userId, teamId);
            return Ok("Team Lead successfully set!");
        }

        private string GenerateLocation(Team newTeam)
        {
            return _linkGenerator.GetPathByAction("Get",
              "Teams",
              new { teamId = newTeam.Id });
        }
    }
}
