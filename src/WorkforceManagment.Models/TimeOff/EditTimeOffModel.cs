using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.TimeOff
{
    [ExcludeFromCodeCoverage]
    public class EditTimeOffModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(240)]
        public string Reason { get; set; }
    }
}
