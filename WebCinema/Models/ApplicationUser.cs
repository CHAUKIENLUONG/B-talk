using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string CMND { get; set; }
    }
}
