using System;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.Models.DTO.Responses
{
    public class TimeOffReponseModel
    {
        public Guid Id { get; set; }
        public TimeOffRequestType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
    }
}
