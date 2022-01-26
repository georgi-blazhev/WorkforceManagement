using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.User
{
    [ExcludeFromCodeCoverage]
    public class CreateUserModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Role { get; set; }
    }
}
