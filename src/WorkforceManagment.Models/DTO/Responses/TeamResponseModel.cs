using System;

namespace WorkforceManagement.Models.DTO.Responses
{
    public class TeamResponseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}