using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagment.Models.DTO.Requests.UserRequests
{
    public class EditUserModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string NewUserName { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
