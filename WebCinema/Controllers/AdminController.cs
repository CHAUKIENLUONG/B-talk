﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCinema.Models;
using WebCinema.Repositories;

namespace WebCinema.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]

    public class AdminController : Controller
    {
        private readonly IMovieRepo _movieRepo;
        private readonly IGenreRepo _genreRepo;
        private readonly IShowtimeRepo _showtimeRepo;
        private readonly IScreentimeRepo _screentimeRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IVoucherRepo _voucherRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        public IActionResult UpdateSelectedDate(string selectedDate)
        {
            // Xử lý logic để cập nhật ngày được chọn ở đây
            // Ví dụ: có thể lưu selectedDate vào session hoặc ViewBag

            return Ok(); // Trả về mã thành công (200)
        }


        public AdminController(ApplicationDbContext context, IMovieRepo movieRepo, IGenreRepo genreRepo, 
            IShowtimeRepo showtimeRepo, IScreentimeRepo screentimeRepo, IRoomRepo roomRepo, IVoucherRepo voucherRepo, IWebHostEnvironment hostingEnvironment)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _showtimeRepo = showtimeRepo;
            _screentimeRepo = screentimeRepo;
            _roomRepo = roomRepo;
            _voucherRepo = voucherRepo;
             _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepo.GetAllAsync();
            return View(movies);
        }
        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            var genres = await _genreRepo.GetAllAsync();
            ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Movie movie, IFormFile poster)
        {
            if (ModelState.IsValid)
            {
                if (poster != null)
                {
                    if (ValidateImageExtension(poster.FileName))
                    {
                        if (!ValidatImageSize(poster, 5242880))
                        {
                            ModelState.AddModelError("Poster", "Image size is too big. The limit is only 5MB");
                            return View(movie);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Poster", "Invalid image format for main image. Please upload a jpg, jpeg, jfif, or png file.");
                        return View(movie);
                    }
                    movie.Poster = await SaveImage(poster);
                }
                await _movieRepo.AddAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Poster", "Please enter a image.");
                // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
                var genres = await _genreRepo.GetAllAsync();
                ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
                return View(movie);
            }
            
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn     
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "~/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        public async Task<IActionResult> Display(int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var Genre = await _genreRepo.GetAllAsync();
            ViewBag.Genres = new SelectList(Genre, "GenreId", "GenreName", movie.GenreId);

            return View(movie);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Movie movie, IFormFile poster)

        {
            ModelState.Remove("Poster"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != movie.MovieId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingMovie = await

                _movieRepo.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
                                             // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (poster == null)
                {
                    movie.Poster = existingMovie.Poster;
                }
                else
                {
                    if (ValidateImageExtension(poster.FileName))
                    {
                        if (!ValidatImageSize(poster, 5242880))
                        {
                            ModelState.AddModelError("Poster", "Image size is too big. The limit is only 1MB");
                            return View(movie);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Poster", "Invalid image format for main image. Please upload a jpg, jpeg, jfif, or png file.");
                        return View(movie);
                    }
                    // Lưu hình ảnh mới
                    movie.Poster = await SaveImage(poster);
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingMovie.MovieName = movie.MovieName;
                existingMovie.GenreId = movie.GenreId;
                existingMovie.MovieLength = movie.MovieLength;
                existingMovie.Description = movie.Description;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.EndDate = movie.EndDate;
                existingMovie.TotalCost = movie.TotalCost;
                existingMovie.Trailer = movie.Trailer;
                existingMovie.MovieState = movie.MovieState;
                existingMovie.Poster = movie.Poster;
                await _movieRepo.UpdateAsync(existingMovie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Poster", "Please enter a image.");
                var genres = await _genreRepo.GetAllAsync();
                ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
                return View(movie);
            }
            
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]  // Add this attribute

        public async Task<IActionResult> DeleteConfirmed(int movieId)
        {
            if (movieId <= 0)  // Check for non-positive values
            {
                return BadRequest("Invalid Movie ID");  // Handle invalid ID
            }
            await _movieRepo.DeleteAsync(movieId);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DashBoard()
        {
            var movies = await _movieRepo.GetAllAsync();
            return View(movies);
        }

        private bool ValidateImageExtension(string fileName)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };
            return allowedExtensions.Contains(Path.GetExtension(fileName).ToLower());
        }
        private bool ValidatImageSize(IFormFile file, long maximumSize)
        {
            return file.Length <= maximumSize;
        }
        public async Task<IActionResult> AddShowtime(Showtime showtime, int movieId) // Use the Showtime model as the parameter
        {
            //showtime.MovieId = movieId;
            var movies = await _movieRepo.GetAllShowAsync(movieId);
            ViewBag.Movies = new SelectList(movies, "MovieId", "MovieName");

            var screentimes = await _screentimeRepo.GetAllAsync();
            ViewBag.Screentimes = new SelectList(screentimes, "ScreenTimeId", "ScreenTime");

            var rooms = await _roomRepo.GetAllAsync();
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomName");

            return View(showtime); // Re-render the view with populated showtime object (for validation errors)
        }
        [HttpPost]
        public async Task<IActionResult> AddShowtime(Showtime showtime, int movieId, string roomId)
        {
            //var movie = await _movieRepo.GetByIdAsync(movieId);
            //var room = await _roomRepo.GetByIdAsync(roomId);
            if (ModelState.IsValid)
            {
                await _showtimeRepo.AddAsync(showtime);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập

                var movies = await _movieRepo.GetAllShowAsync(movieId);
                ViewBag.Movies = new SelectList(movies, "MovieId", "MovieName");

                var screentimes = await _screentimeRepo.GetAllAsync();
                ViewBag.Screentimes = new SelectList(screentimes, "ScreenTimeId", "ScreenTime");

                var rooms = await _roomRepo.GetAllAsync();
                ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomName");
                return View(showtime);
            }
            
        }

        public async Task<IActionResult> IndexVoucher()
        {
            var vouchers = await _voucherRepo.GetAllAsync();
            return View(vouchers);
        }

        // Hiển thị form thêm voucher mới
        public async Task<IActionResult> AddVoucher()
        {
            var vouchers = await _voucherRepo.GetAllAsync();
            ViewBag.Vouchers = new SelectList(vouchers, "Id", "Code");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                await _voucherRepo.AddAsync(voucher);
                return RedirectToAction(nameof(IndexVoucher));
            }
            else
            {
                // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
                var vouchers = await _voucherRepo.GetAllAsync();
                ViewBag.Vouchers = new SelectList(vouchers, "Id", "Code");
                return View(voucher);
            }
        }



        //// Hàm lưu hình ảnh
        //private async Task<string> SaveVoucherImage(IFormFile image)
        //{
        //    var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn     
        //    using (var fileStream = new FileStream(savePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(fileStream);
        //    }
        //    return "~/images/" + image.FileName; // Trả về đường dẫn tương đối
        //}

        public async Task<IActionResult> DisplayVoucher(int id)
        {
            var voucher = await _voucherRepo.GetByIdAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // Hiển thị form cập nhật voucher
        public async Task<IActionResult> UpdateVoucher(int id)
        {
            var voucher = await _voucherRepo.GetByIdAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            var vouchers = await _voucherRepo.GetAllAsync();
            ViewBag.Vouchers = new SelectList(vouchers, "Id", "Code", voucher.Id);

            return View(voucher);
        }

        // Xử lý cập nhật voucher
        [HttpPost]
        public async Task<IActionResult> UpdateVoucher(int id, Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingVoucher = await _voucherRepo.GetByIdAsync(id);

                // Cập nhật các thông tin khác của voucher
                existingVoucher.Code = voucher.Code;
                existingVoucher.Description = voucher.Description;
                existingVoucher.ReleaseDate = voucher.ReleaseDate;
                existingVoucher.EndDate = voucher.EndDate;

                await _voucherRepo.UpdateAsync(existingVoucher);
                return RedirectToAction(nameof(IndexVoucher));
            }
            else
            {
                ModelState.AddModelError("Error", "Please correct the errors in the form.");
                var vouchers = await _voucherRepo.GetAllAsync();
                ViewBag.Vouchers = new SelectList(vouchers, "Id", "Code");
                return View(voucher);
            }
        }

        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await _voucherRepo.GetByIdAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteVoucherConfirmed")]
        [ValidateAntiForgeryToken]  // Add this attribute

        public async Task<IActionResult> DeleteVoucherConfirmed(int Id)
        {
            if (Id <= 0)  // Check for non-positive values
            {
                return BadRequest("Invalid Voucher ID");  // Handle invalid ID
            }
            await _voucherRepo.DeleteAsync(Id);
            return RedirectToAction(nameof(IndexVoucher));
        }
    }
}