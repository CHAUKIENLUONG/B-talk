using System.Collections.Generic;

namespace WebCinema.Models
{
    public class CvModel
    {
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; } // Đường dẫn đến ảnh
        public string Summary { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Experience { get; set; }
        public List<string> Education { get; set; }
    }
}
