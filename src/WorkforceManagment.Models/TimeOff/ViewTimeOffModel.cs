using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.TimeOff
{
    [ExcludeFromCodeCoverage]
    public class ViewTimeOffModel
    {
        public Guid Id { get; set; }
        public string Creator { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ViewDayOffModel> DaysOff { get; set; }
        public string Status { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }
}
