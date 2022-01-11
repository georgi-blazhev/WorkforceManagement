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

        public TimeOffRequestsController(ITimeOffRequestService timeOffRequestService,IUserService userService) : base()
        {
            _timeOffRequestService = timeOffRequestService;
            _userService = userService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<List<TimeOffRequestReponseModel>> GetAll()
        {
            var all = await _timeOffRequestService.GetAllTimeOffsAsync();

            List<TimeOffRequestReponseModel> result = new List<TimeOffRequestReponseModel>();

            if (all == null)

                throw new KeyNotFoundException("There are no time off requests at the moment");

            foreach (var tmr in all)
            {
                result.Add(new TimeOffRequestReponseModel()
                {
                    Id = tmr.Id.ToString(),
                    StartDate = tmr.StartDate,
                    EndDate = tmr.EndDate,
                    Reason = tmr.Reason,
                    Type = tmr.Type

                });
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostTimeOff(CreateTimeOffRequestModel timeOffRequestModel)
        {
            var currentUser = await _userService.GetCurrentUser(User);
            await _timeOffRequestService.CreateTimeOffAsync(timeOffRequestModel,currentUser);
            return NoContent();
        }

        [HttpPut("{Id}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> PutTimeOFf(EditTimeOffRequestModel timeOffRequestModel, string Id)
        {
            await _timeOffRequestService.EditTimeOff(timeOffRequestModel, Id);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        [Authorize("TimeOffRequestAdminOrCreator")]
        public async Task<IActionResult> DeleteTimeOff(string Id)
        {
            await _timeOffRequestService.DeleteTimeOffAsync(Id);
            return NoContent();
        }
    }
}