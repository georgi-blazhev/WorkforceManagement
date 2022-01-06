using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagement.DAL.Entities
{
    public abstract class AbstractEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual User Creator { get; set; }
        public string CreatorId { get; set; }
        public DateTime LastChange { get; set; }
    }
}
