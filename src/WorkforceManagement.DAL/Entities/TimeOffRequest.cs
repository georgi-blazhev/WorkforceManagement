using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagement.DAL.Entities
{
    public class TimeOffRequest : AbstractEntity
    {
        public TimeOffRequestType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
        public virtual List<User> Approvers { get; set; }
    }

    public enum TimeOffRequestType
    {
        Paid, Unpaid, SickLeave
    }

    public enum Status
    {
        Created, Awaiting, Approved, Rejected
    }
}
