using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagement.DAL.Entities
{
    public class UserTimeOffRequest
    {
        public string ApproverId { get; set; }
        public Guid TimeOffRequestId { get; set; }
        public string Decision { get; set; }
    }
}
