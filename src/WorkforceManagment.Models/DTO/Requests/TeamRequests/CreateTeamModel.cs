using System.ComponentModel.DataAnnotations;

namespace WorkforceManagement.Models.DTO.Requests.TeamRequests
{
    public class CreateTeamModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}