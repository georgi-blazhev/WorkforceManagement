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
    [Route("api/teams")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService) : base()
        {
            _teamService = teamService;
        }

        [HttpPost]
        [Route("/create/team")]
        public void Create(CreateTeamModel team)
        {
            if (ModelState.IsValid)
            {
                _teamService.CreateTeam(team.Title);
            }
        }

        [HttpPut]
        [Route("/update/{teamId}")]
        public void Edit(string id, EditTeamModel team)
        {
            if (ModelState.IsValid)
            {
                _teamService.EditTeam(id, team.Title, team.Description);
            }
        }

        [HttpDelete]
        [Route("/delete/{teamId}")]
        public void Delete(string id)
        {
            if (ModelState.IsValid)
            {
                _teamService.DeleteTeam(id);
            }
        }

        [HttpPost]
        [Route("/assign/{userId}")]
        public void AssignUserToTeam(User user, string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.AssignUserToTeam(user, teamId);
            }
        }

        [HttpDelete]
        [Route("/remove/{userId}")]
        public void DeleteUserFromTeam(User user, string teamId)
        {
            if (ModelState.IsValid)
            {
                _teamService.DeleteUserFromTeam(user, teamId);
            }
        }

        [HttpGet]
        [Route("/teams")]
        public async Task<List<TeamResponseModel>> GetAllTeams()
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
        [Route("/members")]
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
    }
}
