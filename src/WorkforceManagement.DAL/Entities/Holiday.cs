using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.DAL.Entities
{
    [ExcludeFromCodeCoverage]
    public class Holiday
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime OfficialHoliday { get; set; }

        public Holiday(DateTime officialHoliday)
        {
            OfficialHoliday = officialHoliday;
        }
    }
}
