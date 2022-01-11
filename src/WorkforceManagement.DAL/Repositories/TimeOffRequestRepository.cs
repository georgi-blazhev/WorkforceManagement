using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public class TimeOffRequestRepository : Repository<TimeOffRequest>, ITimeOffRequestRepository
    {
        public TimeOffRequestRepository(DatabaseContext context)
            : base(context)
        {
        }
        public async Task<List<TimeOffRequest>> GetAllTimeOffsByUser(User user)
        // cannot cast implicitly, FindAsync returns a list anyway shouldnt be a problem
        {
            return (List<TimeOffRequest>)await FindAsync(timeoff => timeoff.CreatorId == user.Id);
        }

        public async Task CreateTimeOffAsync(TimeOffRequest timeOffRequest)
        {
            await CreateAsync(timeOffRequest);
            var teams = _dataContext.Teams.Where(t => t.Members.Any(u=> u.Id == timeOffRequest.CreatorId));
            // will need the newly created timeoffrequest id later on so i search it in the database
             var update = _dataContext.TimeOffRequests.FirstOrDefault(
               t => t.CreatorId == timeOffRequest.CreatorId && t.StartDate == timeOffRequest.StartDate && t.EndDate == timeOffRequest.EndDate);
            foreach (var team in teams)
            {           
                 UserTimeOffRequest utr = new();
                 utr.ApproverId = team.TeamLeaderId;
                 utr.TimeOffRequestId = update.Id;
                 await _dataContext.UserTimeOffRequests.AddAsync(utr);
            }
             await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteTimeOffAsync(TimeOffRequest timeOffRequest)
        {
            var utrs = _dataContext.UserTimeOffRequests.Where(utr => utr.TimeOffRequestId == timeOffRequest.Id);
            foreach (var utr in utrs)
            {
                _dataContext.UserTimeOffRequests.Remove(utr);
            }
            Delete(timeOffRequest);
            await _dataContext.SaveChangesAsync();
        }

        public void EditTimeOff(TimeOffRequest timeOffRequest)
        {
            Edit(timeOffRequest);            
        }
    }
}
