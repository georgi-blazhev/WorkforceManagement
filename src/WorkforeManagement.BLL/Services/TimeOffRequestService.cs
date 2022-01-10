using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.Repositories;
using WorkforceManagement.Models.DTO.Requests.TimeOffRequests;
using WorkforceManagement.Models.DTO.Responses;

namespace WorkforceManagement.BLL.Services
{
    public class TimeOffRequestService : ITimeOffRequestService
    {
        private readonly ITimeOffRequestRepository _timeOffRequestRepository;
        private readonly IUserManager _userManager;
        public TimeOffRequestService(ITimeOffRequestRepository timeOffRequestRepository, IUserManager userManager)
        {
            _timeOffRequestRepository = timeOffRequestRepository;
            _userManager = userManager;
        }

        public async Task CreateTimeOffAsync(CreateTimeOffRequestModel timeOffRequest) // TODO: need current user function so i can assign creatorId
        {                                                                               // need creatorId assgined for Repo to work
            TimeOffRequest newTimeOff = new TimeOffRequest();
            newTimeOff.StartDate = timeOffRequest.StartDate;
            newTimeOff.EndDate = timeOffRequest.EndDate;
            newTimeOff.Reason = timeOffRequest.Reason;
            newTimeOff.Type = timeOffRequest.Type;
            newTimeOff.Status = Status.Created;
            await _timeOffRequestRepository.CreateTimeOffAsync(newTimeOff);
        }
        public async Task DeleteTimeOffAsync(string id)
        {
            var timeOff = await _timeOffRequestRepository.GetTimeOffByIdAsync(id);
            await _timeOffRequestRepository.DeleteTimeOffAsync(timeOff);
        }
        public async Task EditTimeOff(EditTimeOffRequestModel timeOffRequest, string id)
        {
            var originalTimeOff = await _timeOffRequestRepository.GetTimeOffByIdAsync(id);
            originalTimeOff.StartDate = timeOffRequest.StartDate;
            originalTimeOff.EndDate = timeOffRequest.EndDate;
            originalTimeOff.Reason = timeOffRequest.Reason;
            originalTimeOff.Type = timeOffRequest.Type;
            _timeOffRequestRepository.EditTimeOff(originalTimeOff);
        }
        public async Task<List<TimeOffRequestReponseModel>> GetAllTimeOffsAsync()
        {
            var all = await _timeOffRequestRepository.GetAllTimeOffRequest();
            List<TimeOffRequestReponseModel> result = new List<TimeOffRequestReponseModel>();

            foreach (var tmr in all)
            {
                result.Add(new TimeOffRequestReponseModel()
                {
                    StartDate = tmr.StartDate,
                    EndDate = tmr.EndDate,
                    Reason = tmr.Reason,
                    Type = tmr.Type
                });
            }
            return result;
        }
    }
}
