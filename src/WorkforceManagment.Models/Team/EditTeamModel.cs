using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.Team
{
    [ExcludeFromCodeCoverage]
    public class EditTeamModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(240)]
        public string Description { get; set; }
    }
}
