using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Globalization;
using WebCinema.Models;
using WebCinema.Repositories;
using WebCinema.ViewModels;
using ClosedXML.Excel;

namespace WebCinema.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieRepo _movieRepo;
        private readonly IGenreRepo _genreRepo;
        private readonly IShowtimeRepo _showtimeRepo;
        private readonly IScreentimeRepo _screentimeRepo;
        private readonly IRoomRepo _roomRepo;
        //private readonly IBranchRepo _branchRepo;
        /*public DbSet<Branch> Branches { get; set; }*/

        public AdminController(ApplicationDbContext context, IMovieRepo movieRepo, IGenreRepo genreRepo,
            IShowtimeRepo showtimeRepo, IScreentimeRepo screentimeRepo, IRoomRepo roomRepo)
        {
            _context = context;
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _showtimeRepo = showtimeRepo;
            _screentimeRepo = screentimeRepo;
            _roomRepo = roomRepo;
            //_branchRepo = branchRepo;
        }
        // [HttpGet]
        // public async Task<IActionResult> GetMoviesByBranch(int branch)
        // {
        //     // Lấy danh sách phim theo chi nhánh
        //     var movies = await _movieRepo.GetMoviesByBranchAsync(branch); // Giả sử bạn có phương thức này trong repo
        //     return Json(movies); // Trả về danh sách phim dưới dạng JSON
        // }
        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            int pageSize = 10; // Số phim mỗi trang
            var moviesQuery = _context.Movies.AsQueryable();

            // Tìm kiếm phim theo từ khóa nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                moviesQuery = moviesQuery.Where(m => m.MovieName.Contains(searchTerm));
            }

            // Tính số lượng phim tổng cộng
            var totalMovies = await moviesQuery.CountAsync(); // Sử dụng await

            // Lấy phim của trang hiện tại
            var movies = await moviesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính số trang cần thiết
            var totalPages = (int)Math.Ceiling((double)totalMovies / pageSize);

            // Tạo ViewModel
            var model = new MovieIndexViewModel
            {
                Movies = movies,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var genres = await _genreRepo.GetAllAsync();
            ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");

            var branches = await _context.Branches.ToListAsync();
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Movie movie, IFormFile poster)
        {
            if (ModelState.IsValid)
            {
                if (movie.BranchId == 0)
                {
                    ModelState.AddModelError("BranchId", "Vui lòng chọn chi nhánh");
                    return View(movie);
                }

                if (poster != null)
                {
                    if (ValidateImageExtension(poster.FileName) && ValidatImageSize(poster, 5242880))
                    {
                        movie.Poster = await SaveImage(poster);
                    }
                    else
                    {
                        ModelState.AddModelError("Poster", "Định dạng hình ảnh không hợp lệ hoặc kích thước quá lớn.");
                        return View(movie);
                    }
                }

                await _movieRepo.AddAsync(movie);
                TempData["Success"] = "Phim đã được thêm thành công!";
                return RedirectToAction("Index");
            }

            var genres = await _genreRepo.GetAllAsync();
            ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
            var branches = await _context.Branches.ToListAsync();
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName");
            return View(movie);
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
            var branches = await _context.Branches.ToListAsync();
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName", movie.BranchId);

            return View(movie);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Movie movie, IFormFile poster)
        {
            ModelState.Remove("Poster"); // Loại bỏ xác thực ModelState cho Poster
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingMovie = await _movieRepo.GetByIdAsync(id); // Lấy thông tin phim hiện tại từ DB
                if (existingMovie == null)
                {
                    return NotFound();
                }

                // Giữ nguyên hình ảnh nếu không có hình mới được tải lên
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
                            ModelState.AddModelError("Poster", "Image size is too big. The limit is only 5MB");
                            return View(movie);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Poster", "Invalid image format. Please upload a jpg, jpeg, jfif, or png file.");
                        return View(movie);
                    }

                    // Lưu hình ảnh mới
                    movie.Poster = await SaveImage(poster);
                }

                // Cập nhật các thông tin khác của phim
                existingMovie.MovieName = movie.MovieName;
                existingMovie.GenreId = movie.GenreId;
                existingMovie.BranchId = movie.BranchId;
                existingMovie.MovieLength = movie.MovieLength;
                existingMovie.Description = movie.Description;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.EndDate = movie.EndDate;
                existingMovie.Trailer = movie.Trailer;
                existingMovie.Poster = movie.Poster;

                // Tự động xác định trạng thái phim dựa trên ngày phát hành
                var today = DateTime.Now.Date;
                if (movie.ReleaseDate.Date == today)
                {
                    existingMovie.MovieState = "1"; // Đang Chiếu
                }
                else if (movie.ReleaseDate.Date > today)
                {
                    existingMovie.MovieState = "2"; // Sắp Chiếu
                }
                else
                {
                    existingMovie.MovieState = "0"; // Đã Kết Thúc
                }

                await _movieRepo.UpdateAsync(existingMovie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Poster", "Please upload a valid image.");
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
            // Lấy danh sách vé
            var tickets = await GetTicketsAsync();

            // Tổng doanh thu hôm nay
            var totalRevenueToday = await GetTotalRevenueAsync(DateTime.Today);

            // Tổng doanh thu tháng
            var totalRevenueThisMonth = await GetMonthlyRevenueAsync(DateTime.Today);

            // Lấy danh sách phim đang chiếu
            var currentMovies = await GetCurrentMoviesAsync();

            // Lấy dữ liệu vé theo ngày
            var ticketsByDay = await GetTicketsGroupedByDayAsync();

            // Lấy dữ liệu vé theo tháng
            var ticketsByMonth = await GetTicketsGroupedByMonthAsync();

            // Lấy dữ liệu vé theo năm
            var ticketsByYear = await GetTicketsGroupedByYearAsync();

            var chartData = ticketsByDay.Select(t => new
            {
                Date = t.Date.ToString("dd/MM/yyyy"), // Định dạng ngày thành chuỗi
                Count = t.Count
            }).ToList();

            ViewBag.ChartData = chartData; // Truyền dữ liệu qua ViewBag

            // Chuẩn bị dữ liệu gửi đến View
            ViewBag.TotalRevenueToday = totalRevenueToday;
            ViewBag.TotalRevenueThisMonth = totalRevenueThisMonth;
            ViewBag.CurrentMovies = currentMovies;
            ViewBag.TicketsByDay = ticketsByDay;
            ViewBag.TicketsByMonth = ticketsByMonth;
            ViewBag.TicketsByYear = ticketsByYear;

            return View(tickets);
        }

        private async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.Showtime.Movie)
                .OrderByDescending(t => t.PurchaseDate)
                .Take(100) // Giới hạn số vé lấy ra để tránh quá tải
                .ToListAsync();
        }

        private async Task<decimal> GetTotalRevenueAsync(DateTime date)
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue && t.PurchaseDate.Value.Date == date.Date)
                .SumAsync(t => t.Total);
        }

        private async Task<decimal> GetMonthlyRevenueAsync(DateTime date)
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue && t.PurchaseDate.Value.Month == date.Month && t.PurchaseDate.Value.Year == date.Year)
                .SumAsync(t => t.Total);
        }

        private async Task<List<Movie>> GetCurrentMoviesAsync()
        {
            return await _context.Movies
                .Where(m => m.Showtimes.Any(s => s.ShowtimeDate >= DateTime.Today))
                .ToListAsync();
        }

        private async Task<List<object>> GetDailyRevenueDataAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue && t.PurchaseDate.Value.Date >= startDate.Date && t.PurchaseDate.Value.Date <= endDate.Date)
                .GroupBy(t => t.PurchaseDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(t => t.Total)
                })
                .OrderBy(g => g.Date)
                .ToListAsync<object>();
        }

        private async Task<List<dynamic>> GetTicketsGroupedByDayAsync()
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue)
                .GroupBy(t => t.PurchaseDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                    TotalRevenue = g.Sum(t => t.Total)
                })
                .OrderByDescending(g => g.Date)
                .ToListAsync<dynamic>();
        }

        private async Task<List<dynamic>> GetTicketsGroupedByMonthAsync()
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue)
                .GroupBy(t => new { t.PurchaseDate.Value.Year, t.PurchaseDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count(),
                    TotalRevenue = g.Sum(t => t.Total)
                })
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToListAsync<dynamic>();
        }

        private async Task<List<dynamic>> GetTicketsGroupedByYearAsync()
        {
            return await _context.Tickets
                .Where(t => t.PurchaseDate.HasValue)
                .GroupBy(t => t.PurchaseDate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count(),
                    TotalRevenue = g.Sum(t => t.Total)
                })
                .OrderByDescending(g => g.Year)
                .ToListAsync<dynamic>();
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
        [HttpGet]
        public async Task<IActionResult> AddShowtime(int movieId)
        {
            // Lấy thông tin phim đã chọn
            var selectedMovie = await _context.Movies
                .Include(m => m.Branch)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (selectedMovie == null)
            {
                return NotFound();
            }

            var viewModel = new AddShowtimeViewModel
            {
                MovieId = movieId,
                ShowtimeDate = DateTime.Now,
                Branch = selectedMovie.Branch?.BranchName // Lấy tên chi nhánh của phim
            };

            // Chỉ lấy danh sách phòng của chi nhánh đó
            var rooms = await _roomRepo.GetAllAsync();
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomName");

            var screentimes = await _screentimeRepo.GetAllAsync();
            ViewBag.Screentimes = new SelectList(screentimes, "ScreenTimeId", "ScreenTime");

            // Truyền thông tin phim để hiển thị
            ViewBag.SelectedMovie = selectedMovie;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieDetails(int movieId)
        {
            var movie = await _context.Movies
                .Include(m => m.Branch)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (movie == null)
                return NotFound();

            return Json(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddShowtime(AddShowtimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var showtime = new Showtime
                {
                    MovieId = model.MovieId,
                    RoomId = model.RoomId,
                    ShowtimeDate = model.ShowtimeDate,
                    ScreenTimeId = model.ScreenTimeId
                };

                // Kiểm tra trùng lặp
                bool isDuplicate = await _showtimeRepo.IsShowtimeDuplicateAsync(
                    showtime.MovieId, 
                    showtime.RoomId, 
                    showtime.ShowtimeDate, 
                    showtime.ScreenTimeId);

                if (isDuplicate)
                {
                    TempData["ErrorMessage"] = "Lịch chiếu này đã tồn tại!";
                }
                else
                {
                    await _showtimeRepo.AddAsync(showtime);
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu có lỗi, load lại các SelectList
            var movies = await _movieRepo.GetAllShowAsync(model.MovieId);
            ViewBag.Movies = new SelectList(movies, "MovieId", "MovieName");

            var screentimes = await _screentimeRepo.GetAllAsync();
            ViewBag.Screentimes = new SelectList(screentimes, "ScreenTimeId", "ScreenTime");

            var rooms = await _roomRepo.GetAllAsync();
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomName");

            return View(model);
        }



        // Xác nhận thanh toán và cập nhật số lượng bán
        public IActionResult ConfirmPurchase(int comboId, int quantity)
        {
            if (quantity <= 0)
            {
                // Thông báo lỗi nếu số lượng không hợp lệ
                TempData["Error"] = "Số lượng không hợp lệ!";
                return RedirectToAction("InventoryManagement");
            }

            var inventory = _context.Inventories
                .Include(i => i.Combo) // Đảm bảo dữ liệu Combo được tải cùng Inventory
                .FirstOrDefault(i => i.ComboId == comboId);

            if (inventory != null)
            {
                // Cập nhật số lượng bán khi thanh toán
                inventory.QuantitySold += quantity;

                // Lưu thay đổi vào database
                _context.SaveChanges();
            }
            else
            {
                TempData["Error"] = "Combo không tồn tại trong tồn kho!";
            }

            return RedirectToAction("InventoryManagement");
        }

        // Hiển thị trang quản lý tồn kho
        public IActionResult InventoryManagement(string searchQuery)
        {
            // Lưu lịch sử tồn kho hàng ngày
            SaveDailyInventoryHistory();

            // Lấy danh sách Combo từ database
            var combos = _context.Combos.ToList();

            // Truyền danh sách vào ViewBag
            ViewBag.Combos = combos;

            // Các phần xử lý khác (tìm kiếm, lấy tồn kho, v.v.)
            DateTime? searchDate = null;
            if (!string.IsNullOrEmpty(searchQuery) && DateTime.TryParseExact(searchQuery, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                searchDate = result;
            }

            var inventories = _context.Inventories.Include(i => i.Combo).ToList();
            var inventoryEntries = _context.InventoryEntries.Include(e => e.Combo).ToList();

            if (searchDate.HasValue)
            {
                inventoryEntries = inventoryEntries
                    .Where(entry => entry.EntryDate.Date == searchDate.Value.Date)
                    .ToList();
            }

            var comboSales = _context.TicketCombos
                .GroupBy(tc => tc.ComboId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );

            // Lấy số lượng bán của ComboId = 1
            if (comboSales.TryGetValue(1, out int combo1Sales))
            {
                // Cộng số lượng bán của ComboId 1 vào ComboId 2 và 3
                if (comboSales.ContainsKey(2))
                {
                    comboSales[2] += combo1Sales;
                }
                else
                {
                    comboSales[2] = combo1Sales;
                }

                if (comboSales.ContainsKey(3))
                {
                    comboSales[3] += combo1Sales;
                }
                else
                {
                    comboSales[3] = combo1Sales;
                }
            }

            var inventoryData = inventories.Select(inventory => new
            {
                ComboId = inventory.ComboId,
                ComboName = inventory.Combo.ComboName,
                QuantityInStock = inventory.QuantityInStock,
                QuantitySold = comboSales.ContainsKey(inventory.ComboId) ? comboSales[inventory.ComboId] : 0
            }).ToList();

            foreach (var inventory in inventories)
            {
                var salesData = inventoryData.FirstOrDefault(d => d.ComboId == inventory.ComboId);
                if (salesData != null)
                {
                    inventory.QuantitySold = salesData.QuantitySold;
                }
            }

            _context.SaveChanges();

            // Lấy lịch sử tồn kho cuối cùng cho mỗi ComboId
            var latestInventoryHistories = _context.InventoryHistories
                .Include(h => h.Combo)
                .OrderByDescending(h => h.Date)
                .GroupBy(h => h.ComboId)
                .Select(g => g.FirstOrDefault())
                .ToList();

            ViewBag.InventoryEntries = inventoryEntries;
            ViewBag.InventoryData = inventoryData;
            ViewBag.InventoryHistories = latestInventoryHistories;

            return View(inventories);
        }







        // POST: Thêm một lần nhập hàng
        [HttpPost]
        public IActionResult AddInventoryEntry(int comboId, int quantity)
        {
            // Lưu vào lịch sử nhập hàng
            var newEntry = new InventoryEntry
            {
                ComboId = comboId,
                Quantity = quantity,
                EntryDate = DateTime.Now
            };
            _context.InventoryEntries.Add(newEntry);

            // Cập nhật tồn kho
            var inventory = _context.Inventories.FirstOrDefault(i => i.ComboId == comboId);

            if (inventory != null)
            {
                inventory.QuantityInStock += quantity; // Cộng số lượng mới vào
            }
            else
            {
                inventory = new Inventory
                {
                    ComboId = comboId,
                    QuantityInStock = quantity,
                    QuantitySold = 0
                };
                _context.Inventories.Add(inventory);
            }

            _context.SaveChanges();
            return RedirectToAction("InventoryManagement");
        }


        [HttpPost]
        public IActionResult DeleteInventoryEntry(int entryId)
        {
            var entry = _context.InventoryEntries.FirstOrDefault(e => e.InventoryEntryId == entryId);
            if (entry != null)
            {
                // Trừ số lượng nhập trong tồn kho
                var inventory = _context.Inventories.FirstOrDefault(i => i.ComboId == entry.ComboId);
                if (inventory != null)
                {
                    inventory.QuantityInStock -= entry.Quantity; // Trừ số lượng nhập tương ứng
                }

                // Xóa mục nhập hàng
                _context.InventoryEntries.Remove(entry);
                _context.SaveChanges();
            }

            return RedirectToAction("InventoryManagement");
        }

        public void SyncInventoryWithEntries()
        {
            var combos = _context.Combos.ToList();

            foreach (var combo in combos)
            {
                // Tính tổng số lượng nhập từ lịch sử
                var totalQuantity = _context.InventoryEntries
                    .Where(e => e.ComboId == combo.ComboId)
                    .Sum(e => e.Quantity);

                // Cập nhật tồn kho
                var inventory = _context.Inventories.FirstOrDefault(i => i.ComboId == combo.ComboId);
                if (inventory != null)
                {
                    inventory.QuantityInStock = totalQuantity;
                }
                else
                {
                    _context.Inventories.Add(new Inventory
                    {
                        ComboId = combo.ComboId,
                        QuantityInStock = totalQuantity,
                        QuantitySold = 0
                    });
                }
            }

            _context.SaveChanges();
        }

        public void SaveDailyInventoryHistory()
        {
            // Xóa các bản ghi lịch sử tồn kho cũ của ngày hôm nay
            var today = DateTime.Today;
            var oldHistories = _context.InventoryHistories.Where(h => h.Date == today).ToList();
            _context.InventoryHistories.RemoveRange(oldHistories);

            var inventories = _context.Inventories.ToList();
            foreach (var inventory in inventories)
            {
                // Tính số lượng tồn dựa trên yêu cầu
                var remainingQuantity = inventory.QuantityInStock - (inventory.QuantitySold * 500);

                var history = new InventoryHistory
                {
                    ComboId = inventory.ComboId,
                    QuantityInStock = remainingQuantity, // Số lượng tồn mới
                    Date = today
                };
                _context.InventoryHistories.Add(history);
            }
            _context.SaveChanges();
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            var model = new EmployeeModel(); // Khởi tạo đối tượng EmployeeModel
            return View(model);
        }

        // POST: Nhận dữ liệu từ form và thêm nhân viên mới
        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic để thêm hoặc cập nhật nhân viên vào cơ sở dữ liệu
                if (model.EmployeeId == 0) // Nếu là thêm mới
                {
                    _context.Employees.Add(model);
                }
                else // Nếu là cập nhật
                {
                    _context.Employees.Update(model);
                }
                _context.SaveChanges();

                return RedirectToAction("IndexEmployee");
            }
            return View(model);
        }


        // GET: Hiển thị thông tin nhân viên để cập nhật
        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();  // Nếu không tìm thấy nhân viên, trả về lỗi 404
            }
            return View(employee);
        }

        // POST: Nhận dữ liệu từ form và cập nhật thông tin nhân viên
        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeModel updatedEmployee)
        {
            if (ModelState.IsValid)
            {
                var employee = _context.Employees.Find(updatedEmployee.EmployeeId);
                if (employee != null)
                {
                    // Cập nhật thông tin nhân viên
                    employee.FullName = updatedEmployee.FullName;
                    employee.DateOfBirth = updatedEmployee.DateOfBirth;
                    employee.Gender = updatedEmployee.Gender;
                    employee.Position = updatedEmployee.Position;
                    employee.Shift = updatedEmployee.Shift;
                    employee.Status = updatedEmployee.Status;
                    employee.Email = updatedEmployee.Email;
                    employee.PhoneNumber = updatedEmployee.PhoneNumber;
                    employee.Salary = updatedEmployee.Salary;

                    _context.SaveChanges();  // Lưu lại thay đổi
                    TempData["SuccessMessage"] = "Cập nhật thông tin nhân viên thành công!";  // Thông báo thành công
                    return RedirectToAction("IndexEmployee");  // Chuyển hướng về trang danh sách nhân viên
                }
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";  // Thông báo lỗi nếu không tìm thấy
                return RedirectToAction("IndexEmployee");  // Chuyển hướng lại về danh sách
            }

            // Nếu có lỗi, render lại view với dữ liệu đã nhập
            return View(updatedEmployee);
        }


        // GET: Hiển thị thông tin nhân viên để xóa
        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();  // Nếu không tìm thấy nhân viên, trả về lỗi 404
            }
            return View(employee);
        }

        // POST: Xác nhận xóa nhân viên
        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult ConfirmDeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();  // Xóa dữ liệu khỏi cơ sở dữ liệu
                return RedirectToAction("IndexEmployee");  // Chuyển hướng về trang danh sách nhân viên
            }

            return NotFound();  // Trả về lỗi nếu không tìm thấy nhân viên
        }

        // GET: Hiển thị danh sách nhân viên
        public IActionResult IndexEmployee()
        {
            var employees = _context.Employees.ToList(); // Hoặc phương thức phù hợp để lấy danh sách nhân viên
            return View(employees);
        }
        public IActionResult ExportToExcel()
        {
            var employees = _context.Employees.ToList(); // Lấy danh sách nhân viên từ database
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employee List");

            // ặt tiêu đề cho các cột
            worksheet.Cell(1, 1).Value = "Mã Nhân Viên";
            worksheet.Cell(1, 2).Value = "Họ và Tên";
            worksheet.Cell(1, 3).Value = "Ngày Sinh";
            worksheet.Cell(1, 4).Value = "Giới Tính";
            worksheet.Cell(1, 5).Value = "Bộ Phận";
            worksheet.Cell(1, 6).Value = "Ca Làm Việc";
            worksheet.Cell(1, 7).Value = "Trạng Thái";
            worksheet.Cell(1, 8).Value = "Email";
            worksheet.Cell(1, 9).Value = "Số Điện Thoại";
            worksheet.Cell(1, 10).Value = "Lương";

            int row = 2;
            decimal totalSalary = 0; // Khai báo biến tổng lương

            foreach (var employee in employees)
            {
                worksheet.Cell(row, 1).Value = employee.EmployeeId;
                worksheet.Cell(row, 2).Value = employee.FullName;

                // Kiểm tra giá trị DateTime hợp lệ
                if (employee.DateOfBirth != default(DateTime) && employee.DateOfBirth.Year > 1753)
                {
                    worksheet.Cell(row, 3).Value = employee.DateOfBirth.ToString("dd/MM/yyyy");
                }
                else
                {
                    worksheet.Cell(row, 3).Value = "Ngày không hợp lệ"; // Giá trị ghi chú nếu không hợp lệ
                }

                worksheet.Cell(row, 4).Value = employee.Gender;
                worksheet.Cell(row, 5).Value = employee.Position;
                worksheet.Cell(row, 6).Value = employee.Shift;
                worksheet.Cell(row, 7).Value = employee.Status;
                worksheet.Cell(row, 8).Value = employee.Email;
                worksheet.Cell(row, 9).Value = employee.PhoneNumber;

                // Cộng dồn lương
                totalSalary += employee.Salary;

                // Định dạng lương với VND
                worksheet.Cell(row, 10).Value = employee.Salary.ToString("#,##0.00 VND");

                row++;
            }

            // Thêm tổng lương vào dòng tiếp theo
            worksheet.Cell(row, 9).Value = "Tổng Lương"; // Ghi chú
            worksheet.Cell(row, 10).Value = totalSalary.ToString("#,##0.00 VND"); // Tổng lương

            // Tự động điều chỉnh kích thước cột
            worksheet.Columns().AdjustToContents();

            // Xuất file Excel
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeList.xlsx");
            }
        }
        public IActionResult CvIndex()
        {
            var cvModel = new CvModel
            {
                FullName = "Nguyễn Văn A",
                DateOfBirth = "01/01/1990",
                Email = "email@example.com",
                PhoneNumber = "0123456789",
                Address = "Hà Nội",
                ProfilePicture = "/images/profile.jpg", // Đường dẫn ảnh
                Summary = "Một mô tả ngắn về bản thân.",
                Skills = new List<string> { "C# ", "ASP.NET Core", "SQL" },
                Experience = new List<string> { "Công ty ABC - Lập trình viên", "Công ty XYZ - Quản lý dự án" },
                Education = new List<string> { "Đại học B - Khoa CNTT" }
            };

            return View(cvModel);
        }

        [HttpPost]
        public IActionResult Index(CvModel model)
        {
            // Xử lý lưu trữ thông tin CV
            return RedirectToAction("Index");
        }

        // Hiển thị danh sách chi nhánh
        public async Task<IActionResult> BranchIndex()
        {
            var branches = await _context.Branches.ToListAsync();
            return View(branches);
        }

        // Hiển thị form thêm chi nhánh
        [HttpGet]
        public IActionResult AddBranch()
        {
            return View();
        }

        // Xử lý thêm chi nhánh
        [HttpPost]
        public async Task<IActionResult> AddBranch(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Branches.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BranchIndex));
            }
            return View(branch);
        }

        // Hiển thị form sửa chi nhánh
        [HttpGet]
        public async Task<IActionResult> EditBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // Xử lý sửa chi nhánh
        [HttpPost]
        public async Task<IActionResult> EditBranch(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Branches.Update(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BranchIndex));
            }
            return View(branch);
        }

        // Xóa chi nhánh
        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BranchIndex));
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesByBranch(int branchId)
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Where(m => m.BranchId == branchId)
                .Select(m => new { 
                    movieId = m.MovieId, 
                    movieName = m.MovieName 
                })
                .ToListAsync();

            return Json(movies);
        }
    }


}