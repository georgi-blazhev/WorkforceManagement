using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.DAL.Entities
{
    [ExcludeFromCodeCoverage]
    public class Team : AbstractEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual User TeamLeader { get; set; }
        public string TeamLeaderId { get; set; }
        public virtual List<User> Members { get; set; }
    }
}
