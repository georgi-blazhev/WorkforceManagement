﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/timeoffrequests")]
    [ApiController]
    public class TimeOffRequestsController : ControllerBase
    {
        private readonly ITimeOffRequestService _timeOffRequestService;

        public TimeOffRequestsController(ITimeOffRequestService timeOffRequestService) : base()
        {
            _timeOffRequestService = timeOffRequestService;
        }

        [HttpGet]
        public async Task<List<TimeOffRequestReponseModel>> GetAll()
        {
            return await _timeOffRequestService.GetAllTimeOffsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostTimeOff(CreateTimeOffRequestModel timeOffRequestModel)
        {
            await _timeOffRequestService.CreateTimeOffAsync(timeOffRequestModel);
            return NoContent();
        }

        [HttpPut("{Id}")]        
        public async Task<IActionResult> PutTimeOFf(EditTimeOffRequestModel timeOffRequestModel, string Id)
        {
            await _timeOffRequestService.EditTimeOff(timeOffRequestModel, Id);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTimeOff(string Id)
        {
            await _timeOffRequestService.DeleteTimeOffAsync(Id);
            return NoContent();
        }
    }
}