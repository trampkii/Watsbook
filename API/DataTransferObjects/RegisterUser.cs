using System;
using System.ComponentModel.DataAnnotations;

namespace API.DataTransferObjects
{
    public class RegisterUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must have at least 4 characters")]
        public string Password { get; set; }
    }
}