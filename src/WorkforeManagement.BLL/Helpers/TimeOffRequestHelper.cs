using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IHelpers;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.Repositories;

namespace WorkforceManagement.BLL.Helpers
{
    public class TimeOffRequestHelper : ITimeOffRequestHelper
    {
        private readonly ITimeOffRequestRepository _timeOffRequestRepository;

        public TimeOffRequestHelper(ITimeOffRequestRepository timeOffRequestRepository)
        {
            _timeOffRequestRepository = timeOffRequestRepository;
        }
        public TimeOffRequestType FormatType(string type)
        {
            type = type.ToUpper();
            if (type.Equals(TimeOffRequestType.Paid.ToString().ToUpper()))
            {
                return TimeOffRequestType.Paid;
            }
            else if (type.Equals(TimeOffRequestType.Unpaid.ToString().ToUpper()))
            {
                return TimeOffRequestType.Unpaid;
            }
            else if (type.Equals(TimeOffRequestType.SickLeave.ToString().ToUpper()))
            {
                return TimeOffRequestType.SickLeave;
            }
            throw new ArgumentException("The Type can only be Paid, Unpaid or SickLeave! ");
        }
        public Decision FormatDecision(string decision)
        {
            decision = decision.ToUpper().Trim();
            if (decision.Equals(Decision.Approve.ToString().ToUpper()))
            {
                return Decision.Approve;
            }
            else if (decision.Equals(Decision.Reject.ToString().ToUpper()))
            {
                return Decision.Reject;
            }
            throw new ArgumentException("The Decision can only be to Approve or Reject! ");
        }
        public async Task CheckOverlapping(TimeOffRequest newTimeOff, User creator)
        {
            bool isAdminEditingSomeoneElse = false;
            if (newTimeOff.Id.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var timeOffBeforeUpdate = await _timeOffRequestRepository.FindByIdAsync(newTimeOff.Id.ToString());
                if (timeOffBeforeUpdate.Creator != creator)
                    isAdminEditingSomeoneElse = true;
            }

            var currentTimeOffs = await _timeOffRequestRepository.GetAllTimeOffsByUser(creator);
            
            //CHECK: Allow overlap only if existing time off is rejected
            bool newTimeOffStartsDuringAnAlreadyExistingNotRejectedTimeOff = currentTimeOffs
                .Any(existing => existing.StartDate <= newTimeOff.StartDate                    
                && newTimeOff.StartDate <= existing.EndDate && existing.Status != Status.Rejected
                && newTimeOff.Id != existing.Id && (!isAdminEditingSomeoneElse));

            bool notRejectedExistingTimeOffStartsDuringNewTimeOff = currentTimeOffs
                .Any(existing => newTimeOff.StartDate <= existing.StartDate
                && existing.StartDate <= newTimeOff.EndDate && existing.Status != Status.Rejected
                && newTimeOff.Id != existing.Id && (!isAdminEditingSomeoneElse));

            if (currentTimeOffs != null &&
                newTimeOffStartsDuringAnAlreadyExistingNotRejectedTimeOff ||
                notRejectedExistingTimeOffStartsDuringNewTimeOff)
                throw new ArgumentException("Time period overlaps with another time off request! ");
        }
        public async Task<List<DayOff>> GetDaysOff(TimeOffRequest timeOffRequest)
        {
            List<DayOff> daysOff = new();
            List<Holiday> holidays = await _timeOffRequestRepository.GetAllOfficialHolidays();

            if (timeOffRequest.Type != TimeOffRequestType.SickLeave)
            {
                for (DateTime i = timeOffRequest.StartDate.Date; i <= timeOffRequest.EndDate.Date; i = i.AddDays(1))
                {
                    var holiday = holidays.FirstOrDefault(h => h.OfficialHoliday == i);

                    if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday && holiday == null)
                    {
                        daysOff.Add(new DayOff(i));
                    }
                }
            }
            else
            {
                for (DateTime i = timeOffRequest.StartDate.Date; i <= timeOffRequest.EndDate.Date; i = i.AddDays(1))
                {
                    daysOff.Add(new DayOff(i));
                }
            }

            return daysOff;
        }
    }
}
