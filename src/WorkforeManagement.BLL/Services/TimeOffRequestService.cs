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
        public TimeOffRequestService(ITimeOffRequestRepository timeOffRequestRepository)
        {
            _timeOffRequestRepository = timeOffRequestRepository;           
        }

        public async Task CreateTimeOffAsync(CreateTimeOffRequestModel timeOffRequest,User user) // TODO: need current user function so i can assign creatorId
        {            
            if (timeOffRequest.StartDate > timeOffRequest.EndDate)
                throw new ArgumentException("Start date can not be later than End date");
            if (user == null)
                throw new ArgumentException("Invalid user");
            var currentTimeOffs = await _timeOffRequestRepository.GetAllTimeOffsByUser(user);
            if (currentTimeOffs != null &&  currentTimeOffs.Any(t =>
              (t.StartDate > timeOffRequest.StartDate && t.StartDate < timeOffRequest.EndDate && t.Status == Status.Approved) ||
              // user already has an approved timeoffrequest that starts during the new req
              t.EndDate > timeOffRequest.StartDate && t.EndDate < timeOffRequest.EndDate && t.Status == Status.Approved))
                // user already has an approved timeoffreq that ends during new req
                throw new ArgumentException("Time period overlaps with another time off request");
            TimeOffRequest newTimeOff = new TimeOffRequest();
            newTimeOff.StartDate = timeOffRequest.StartDate;
            newTimeOff.EndDate = timeOffRequest.EndDate;
            newTimeOff.Reason = timeOffRequest.Reason;
            newTimeOff.Type = timeOffRequest.Type;
            newTimeOff.Status = Status.Created;
            newTimeOff.CreatedAt = DateTime.Now;
            newTimeOff.LastChange = DateTime.Now;
            newTimeOff.CreatorId = user.Id;
            await _timeOffRequestRepository.CreateTimeOffAsync(newTimeOff);
        }
        public async Task DeleteTimeOffAsync(string id)
        {
            var timeOff = await _timeOffRequestRepository.GetTimeOffByIdAsync(id);
            if (timeOff == null)
                throw new ArgumentException("Invalid time off id");
            await _timeOffRequestRepository.DeleteTimeOffAsync(timeOff);
        }
        public async Task EditTimeOff(EditTimeOffRequestModel timeOffRequest, string id)
        { // check dates 
            if (timeOffRequest.StartDate > timeOffRequest.EndDate)
                throw new ArgumentException("Start date can not be later than End date");            
            var originalTimeOff = await _timeOffRequestRepository.GetTimeOffByIdAsync(id);
            if (originalTimeOff == null)
                throw new ArgumentException("Invalid time off id");
            originalTimeOff.StartDate = timeOffRequest.StartDate;
            originalTimeOff.EndDate = timeOffRequest.EndDate;
            originalTimeOff.Reason = timeOffRequest.Reason;
            originalTimeOff.Type = timeOffRequest.Type;
            originalTimeOff.LastChange = DateTime.Now;
            _timeOffRequestRepository.EditTimeOff(originalTimeOff);
                
        }
        public async Task<List<TimeOffRequestReponseModel>> GetAllTimeOffsAsync()
        {
            var all = await _timeOffRequestRepository.GetAllTimeOffRequest();
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
    }
}
