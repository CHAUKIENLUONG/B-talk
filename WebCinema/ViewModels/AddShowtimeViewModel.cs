namespace WebCinema.ViewModels
{
    public class AddShowtimeViewModel
    {
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateTime ShowtimeDate { get; set; }
        public int ScreenTimeId { get; set; }
        public string Branch { get; set; } // Thêm thuộc tính Branch
    }
}
