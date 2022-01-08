using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagment.Models.DTO.Requests.UserRequests
{
    public class LoginUserModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
