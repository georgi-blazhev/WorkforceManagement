﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.DTO.Requests.TimeOffRequests;
using WorkforceManagement.Models.DTO.Responses;

namespace WorkforceManagement.BLL.Services
{
    public interface ITimeOffRequestService
    {
        Task CreateTimeOffAsync(CreateTimeOffRequestModel timeOffRequest,User user);
        Task DeleteTimeOffAsync(string id);
        Task EditTimeOff(EditTimeOffRequestModel timeOffRequest, string id);
        Task<List<TimeOffRequest>> GetAllTimeOffsAsync();
    }
}