using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.Models.User;

namespace WorkforceManagement.Models.Team
{
    [ExcludeFromCodeCoverage]
    public class ViewTeamDetailModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TeamLeader { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public List<ViewUserModel> Members { get; set; }
    }
}
