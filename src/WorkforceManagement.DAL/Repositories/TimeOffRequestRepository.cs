using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;


namespace WorkforceManagement.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TimeOffRequestRepository : Repository<TimeOffRequest>, ITimeOffRequestRepository
    {
        public TimeOffRequestRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<TimeOffRequest>> GetAllTimeOffsByUser(User user)
        {
            return await FindAsync(timeoff => timeoff.CreatorId == user.Id);
        }
        public async Task<List<Holiday>> GetAllOfficialHolidays()
        {
            return await _dataContext.OfficialHolidays.ToListAsync();
        }
        public async Task CreateTimeOffAsync(TimeOffRequest timeOffRequest)
        {
            await CreateAsync(timeOffRequest);
            var justCreatedTimeOffRequest = _dataContext.TimeOffRequests.FirstOrDefault(t => t.CreatedAt == timeOffRequest.CreatedAt);
           
            if (justCreatedTimeOffRequest.Type == TimeOffRequestType.SickLeave)
            {
                await HandleSickLeaveRequest(justCreatedTimeOffRequest);
                return;
            }

            await AddApprovers(timeOffRequest);
            await _dataContext.SaveChangesAsync();

            if (justCreatedTimeOffRequest.Approvers == null || justCreatedTimeOffRequest.Approvers.Count == 0)
            {       
                justCreatedTimeOffRequest.Status = Status.Approved;
                await _dataContext.SaveChangesAsync();
            }
        }
        public async Task RegisterDecision(TimeOffRequest timeOff, User currentUser, Decision decision)
        {
            var decisionEntry = _dataContext.UserTimeOffRequests
                .Where(e => e.TimeOffRequestId == timeOff.Id)
                .FirstOrDefault(e => e.ApproverId == currentUser.Id);
            
            if (decision == Decision.Reject)
            {
                decisionEntry.Decision = Decision.Reject;
            }
            else
            {
                decisionEntry.Decision = Decision.Approve;
                timeOff.Status = Status.Awaiting;
            }
            await _dataContext.SaveChangesAsync();
        }
        public async Task RegisterRequestStatusChangeIfNeeded(TimeOffRequest timeOff, User currentUser)
        {
            var decisionEntries = _dataContext.UserTimeOffRequests.Where(e => e.TimeOffRequestId == timeOff.Id);
            
            if (decisionEntries.All(e => e.Decision == Decision.Approve))
            {
                timeOff.Status = Status.Approved;
                _dataContext.RemoveRange(decisionEntries);
            }
            else if (decisionEntries.Any(e => e.Decision == Decision.Reject))
            {
                timeOff.Status = Status.Rejected;
                _dataContext.RemoveRange(decisionEntries);
                timeOff.Approvers.Remove(currentUser);
            }
            await _dataContext.SaveChangesAsync();
        }

        // Helper methods
        private async Task HandleSickLeaveRequest(TimeOffRequest timeOff)
        {
            var teamsOfCurrentUser = _dataContext.Teams.Where(t => t.Members.Any(u => u.Id == timeOff.CreatorId)).ToList();
            
            timeOff.Approvers = new List<User>();
            foreach (var teamOfCurrentUser in teamsOfCurrentUser)
            {                
                timeOff.Approvers.Add(teamOfCurrentUser.TeamLeader);
                await _dataContext.SaveChangesAsync();
            }

            timeOff.Status = Status.Approved;

            await _dataContext.SaveChangesAsync();
        }
        private async Task AddApprovers(TimeOffRequest timeOff)
        {
            List<TimeOffRequest> allTimeOffs = await GetAllAsync();
            var teamsOfCurrentUser = _dataContext.Teams.Where(t => t.Members.Any(u => u.Id == timeOff.CreatorId)).ToList();
            foreach (var team in teamsOfCurrentUser)
            {
                var currentLeader = team.TeamLeader;

                bool currentLeaderIsOff = allTimeOffs
                    .Where(t => t.CreatorId == currentLeader.Id)
                    .Any(t => t.StartDate <= DateTime.Now.Date && t.EndDate >= DateTime.Now && t.Status == Status.Approved);

                if (currentLeaderIsOff)
                    continue;
                
                if (timeOff.CreatorId == currentLeader.Id)
                    continue;

                var requestsRequiringDecision = currentLeader.RequestsRequiringDecision;

                requestsRequiringDecision.Add(timeOff);
            }
            await _dataContext.SaveChangesAsync();
        }
    }   
}
