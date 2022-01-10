using System;
using System.ComponentModel.DataAnnotations;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.Models.DTO.Requests.TimeOffRequests
{
    public class CreateTimeOffRequestModel
    {
        [Required]
        public TimeOffRequestType Type { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }
    }
}
