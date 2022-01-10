using System.ComponentModel.DataAnnotations;

namespace WorkforceManagement.Models.DTO.Requests.TeamRequests
{
    public class EditTeamModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}