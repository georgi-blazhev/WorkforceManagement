using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.Repositories
{
    public interface ITimeOffRequestRepository
    {
        Task CreateTimeOffAsync(TimeOffRequest timeOffRequest);
        Task DeleteTimeOffAsync(TimeOffRequest timeOffRequest);
        Task EditTimeOffAsync(TimeOffRequest timeOffRequest);
        Task<List<TimeOffRequest>> GetAllTimeOffRequest();
        Task<List<TimeOffRequest>> GetAllTimeOffsByUser(User user);
        Task<TimeOffRequest> GetTimeOffByIdAsync(string id);
    }
}