using System;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.DAL.Entities
{
    [ExcludeFromCodeCoverage]
    public abstract class AbstractEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User Creator { get; set; }
        public string CreatorId { get; set; }
        public DateTime LastChange { get; set; }
    }
}
