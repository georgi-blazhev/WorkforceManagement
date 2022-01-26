using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Services
{
    public interface ITimeOffRequestService
    {
        Task<TimeOffRequest> GetTimeOffByIdAsync(string timeOffId);
        Task<IEnumerable<TimeOffRequest>> GetAllTimeOffsAsync();
        Task<IEnumerable<TimeOffRequest>> GetTimeOffsByUserAsync(User user);
        Task<TimeOffRequest> CreateTimeOffAsync(TimeOffRequest newTimeOff, string type, User creator);
        Task EditTimeOffAsync(TimeOffRequest timeOffRequest, User creator);
        Task DeleteTimeOffAsync(string timeOffId);
        Task DecideTimeOffAsync(User user, string id, string decision);
        Task<List<TimeOffRequest>> RequireDecisionCurrentUserAsync(User currentUser);
    }
}