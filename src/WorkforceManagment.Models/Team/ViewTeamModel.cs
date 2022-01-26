using System;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.Team
{
    [ExcludeFromCodeCoverage]
    public class ViewTeamModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TeamLeader { get; set; }
    }
}
