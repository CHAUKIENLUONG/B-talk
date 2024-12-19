using System;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.ViewModels
{
    public class TicketViewModel
    {
        [Required(ErrorMessage = "Tên phim là bắt buộc.")]
        public string MovieName { get; set; }

        [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "Thời gian chiếu là bắt buộc.")]
        public TimeSpan ScreenTime { get; set; }

        [Required(ErrorMessage = "Ngày mua là bắt buộc.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Ngày chiếu là bắt buộc.")]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "ID buổi chiếu là bắt buộc.")]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc.")]
        public int TotalMoney { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
    }
}
