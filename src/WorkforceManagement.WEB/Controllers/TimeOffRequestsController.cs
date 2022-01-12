using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.Models.DTO.Requests.TimeOffRequests;
using WorkforceManagement.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagment.Models.DTO.Responses;
using WorkforceManagement.BLL.Services;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeOffRequestsController : ControllerBase
    {
        private readonly ITimeOffRequestService _timeOffRequestService;
        private readonly IUserService _userService;

        public TimeOffRequestsController(ITimeOffRequestService timeOffRequestService, IUserService userService) : base()
        {
            _timeOffRequestService = timeOffRequestService;
            _userService = userService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<List<TimeOffReponseModel>> GetAll()
        {
            var allTimeOffs = await _timeOffRequestService.GetAllTimeOffsAsync();
            List<TimeOffReponseModel> result = new();

            foreach (var timeOff in allTimeOffs)
            {
                result.Add(new TimeOffReponseModel()
                {
                    Id = timeOff.Id,
                    StartDate = timeOff.StartDate,
                    EndDate = timeOff.EndDate,
                    Reason = timeOff.Reason,
                    Type = timeOff.Type
                });
            }
            return result;
        }
        [HttpGet]
        [Route("CurrentUser")]
        public async Task<List<TimeOffReponseModel>> GetAllCurrentUser()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            var allTimeOffs = await _timeOffRequestService.GetTimeOffsByUserAsync(currentUser);
            List<TimeOffReponseModel> result = new();

            foreach (var timeOff in allTimeOffs)
            {
                result.Add(new TimeOffReponseModel()
                {
                    Id = timeOff.Id,
                    StartDate = timeOff.StartDate,
                    EndDate = timeOff.EndDate,
                    Reason = timeOff.Reason,
                    Type = timeOff.Type
                });
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTimeOffModel timeOffRequestModel)
        {
            var currentUser = await _userService.GetCurrentUser(User);
            await _timeOffRequestService.CreateTimeOffAsync(timeOffRequestModel, currentUser);
            return NoContent(); // TODO: Maybe it would be nice to return 201 Created on all our posts?
        }

        [HttpPut("{timeOffId}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> Edit(EditTimeOffModel timeOffRequestModel, string timeOffId)
        {
            await _timeOffRequestService.EditTimeOffAsync(timeOffRequestModel, timeOffId);
            return NoContent();
        }

        [HttpDelete("{timeOffId}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> Delete(string timeOffId)
        {
            await _timeOffRequestService.DeleteTimeOffAsync(timeOffId);
            return NoContent();
        }
    }
}