using System;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.DAL.Entities
{
    [ExcludeFromCodeCoverage]
    public class UserTimeOffRequest
    {
        public string ApproverId { get; set; }
        public Guid TimeOffRequestId { get; set; }
        public Decision Decision { get; set; }
    }
    public enum Decision
    {
        Unknown, Approve, Reject
    }
}
