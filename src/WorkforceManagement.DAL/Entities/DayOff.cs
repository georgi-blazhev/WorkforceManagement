using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.DAL.Entities
{
    [ExcludeFromCodeCoverage]
    public class DayOff
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfDayOff { get; set; }
        public Guid TimeOffRequestId { get; set; }

        public DayOff(DateTime dateOfDayOff)
        {
            DateOfDayOff = dateOfDayOff;
        }
    }
}
