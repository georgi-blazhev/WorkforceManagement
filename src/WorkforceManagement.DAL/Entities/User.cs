using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagement.DAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<TimeOffRequest> RequestsRequiringDecision { get; set; }
    }
}
