using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public interface ITimeOffRequestRepository : IRepository<TimeOffRequest>
    {
        Task<IEnumerable<TimeOffRequest>> GetAllTimeOffsByUser(User user);        
        Task<List<Holiday>> GetAllOfficialHolidays();
        Task CreateTimeOffAsync(TimeOffRequest timeOffRequest);
        Task RegisterDecision(TimeOffRequest timeOff, User currentUser, Decision decisionEntry);
        Task RegisterRequestStatusChangeIfNeeded(TimeOffRequest timeOff, User currentUser);
    }
}