using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.IHelpers
{
    public interface ITimeOffRequestHelper
    {
        TimeOffRequestType FormatType(string type);
        Decision FormatDecision(string decision);
        Task CheckOverlapping(TimeOffRequest newTimeOff, User creator);
        Task<List<DayOff>> GetDaysOff(TimeOffRequest timeOffRequest);
    }
}
