using System;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class EmployeeModel
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Shift { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}
