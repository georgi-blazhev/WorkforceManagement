using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagement.BLL.IServices;
using System.Threading.Tasks;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.Models.TimeOff;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using WorkforceManagement.DAL.Entities;
using System;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TimeOffRequestsController : ControllerBase
    {
        private readonly ITimeOffRequestService _timeOffRequestService;
        private readonly IUserService _userService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public TimeOffRequestsController(ITimeOffRequestService timeOffRequestService, IUserService userService, IMapper mapper, LinkGenerator linkGenerator)
        {
            _timeOffRequestService = timeOffRequestService;
            _userService = userService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{timeOffId}")]
        public async Task<ActionResult<ViewTimeOffModel>> Get(string timeOffId)
        {
            var timeOff = await _timeOffRequestService.GetTimeOffByIdAsync(timeOffId);
            return _mapper.Map<ViewTimeOffModel>(timeOff);
        }

        [HttpGet]
        [Route("All")]
        public async Task<ViewTimeOffModel[]> GetAll()
        {
            var allTimeOffs = await _timeOffRequestService.GetAllTimeOffsAsync();
            return _mapper.Map<ViewTimeOffModel[]>(allTimeOffs);
        }

        [HttpGet]
        [Route("CurrentUser")]
        public async Task<ViewTimeOffModel[]> GetAllCurrentUser()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            var currentUserTimeOffs = await _timeOffRequestService.GetTimeOffsByUserAsync(currentUser);
            return _mapper.Map<ViewTimeOffModel[]>(currentUserTimeOffs);
        }

        [HttpGet]
        [Route("RequireDecisionFromCurrentUser")]
        public async Task<ViewTimeOffModel[]> RequestsThatRequireDecision()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            var RequestRequireDecisionCurrentUser = await _timeOffRequestService.RequireDecisionCurrentUserAsync(currentUser);
            return _mapper.Map<ViewTimeOffModel[]>(RequestRequireDecisionCurrentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTimeOffModel model)
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);

            var newTimeOff = _mapper.Map<TimeOffRequest>(model);
            newTimeOff = await _timeOffRequestService.CreateTimeOffAsync(newTimeOff, model.TimeOffType, currentUser);

            string location = GenerateLocation(newTimeOff);
            if (string.IsNullOrWhiteSpace(location)) return BadRequest("Error at URI creation! ");

            return Created(location, _mapper.Map<ViewTimeOffModel>(newTimeOff));
        }

        [HttpPut("{timeOffId}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> Edit(EditTimeOffModel model, string timeOffId)
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);

            var timeOffWithUpdates = _mapper.Map<TimeOffRequest>(model);
            timeOffWithUpdates.Id = Guid.Parse(timeOffId);
            await _timeOffRequestService.EditTimeOffAsync(timeOffWithUpdates, currentUser);

            return Ok("Time Off Request was successfully edited! ");
        }

        [HttpDelete("{timeOffId}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> Delete(string timeOffId)
        {
            await _timeOffRequestService.DeleteTimeOffAsync(timeOffId);
            return Ok("Time Off Request was successfully deleted! ");
        }

        [HttpPut("{timeOffId}&{decision}")]
        public async Task<IActionResult> MakeDecision(string timeOffId, string decision)
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            await _timeOffRequestService.DecideTimeOffAsync(currentUser, timeOffId, decision);
            return Ok("Your decision was registered! ");
        }

        private string GenerateLocation(TimeOffRequest newTimeOff)
        {
            return _linkGenerator.GetPathByAction("Get",
                        "TimeOffRequests",
                        new { timeOffId = newTimeOff.Id });
        }
    }
}