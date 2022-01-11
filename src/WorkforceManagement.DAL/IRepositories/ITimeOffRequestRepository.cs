using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public interface ITimeOffRequestRepository : IRepository<TimeOffRequest>
    {
        Task CreateTimeOffAsync(TimeOffRequest timeOffRequest);
        Task DeleteTimeOffAsync(TimeOffRequest timeOffRequest);
        void EditTimeOff(TimeOffRequest timeOffRequest);
        Task<List<TimeOffRequest>> GetAllTimeOffsByUser(User user);
    }
}