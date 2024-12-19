using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FullName { get; set; }
    }
}
