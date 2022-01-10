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
        private readonly DatabaseContext _context;
        private readonly ITeamRepository<Team> _teamRepository;

        public TimeOffRequestRepository(DatabaseContext context, ITeamRepository<Team> teamRepository)
            : base(context)
        {
            _context = context;
            _teamRepository = teamRepository;
        }

        public async Task<List<TimeOffRequest>> GetAllTimeOffRequest()
        {
            return await GetAllAsync();
        }
        public async Task<TimeOffRequest> GetTimeOffByIdAsync(string id)
        {
            return await FindByIdAsync(id);
        }
        public async Task<List<TimeOffRequest>> GetAllTimeOffsByUser(User user)
        // cannot cast implicitly, FindAsync returns a list anyway shouldnt be a problem
        {
            return (List<TimeOffRequest>)await FindAsync(timeoff => timeoff.CreatorId == user.Id);
        }

        public async Task CreateTimeOffAsync(TimeOffRequest timeOffRequest)
        {
            await CreateAsync(timeOffRequest);
            var teams = await _teamRepository.FindAsync(t => t.Members.Contains(timeOffRequest.Creator));
            // will need the newly created timeoffrequest id later on so i search it in the database
             var update = _context.TimeOffRequests.FirstOrDefault(
               t => t.CreatorId == timeOffRequest.CreatorId && t.StartDate == timeOffRequest.StartDate && t.EndDate == timeOffRequest.EndDate);
            foreach (var team in teams)
            {           
                 UserTimeOffRequest utr = new();
                 utr.ApproverId = team.TeamLeaderId;
                 utr.TimeOffRequestId = update.Id;
                 await _context.UserTimeOffRequests.AddAsync(utr);
            }
             await SaveChangesAsync();
        }

        public async Task DeleteTimeOffAsync(TimeOffRequest timeOffRequest)
        {
            var utrs = _context.UserTimeOffRequests.Where(utr => utr.TimeOffRequestId == timeOffRequest.Id);
            foreach (var utr in utrs)
            {
                _context.UserTimeOffRequests.Remove(utr);
            }
            Delete(timeOffRequest);
            await SaveChangesAsync();
        }

        public void EditTimeOff(TimeOffRequest timeOffRequest)
        {
            Edit(timeOffRequest);            
        }



    }
}
