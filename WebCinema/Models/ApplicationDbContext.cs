using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;
using System.Net.Sockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebCinema.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Screentime> Screentimes { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketInfo> TicketInfos { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<TicketCombo> TicketCombos { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Showtime)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.ShowId)
                .HasPrincipalKey(s => s.ShowId)
                .IsRequired();

            // Configure the length of the ShowId property in the Ticket entity
            modelBuilder.Entity<Ticket>()
                .Property(t => t.ShowId)
                .HasMaxLength(450);

            //modelBuilder.Entity<IdentityUser>().Ignore(c => c.TwoFactorEnabled);//and so on...
            modelBuilder.Entity<Voucher>()
          .ToTable("Vouchers")  // Đặt tên bảng trong cơ sở dữ liệu
          .HasKey(v => v.Id);  // Chỉ định khóa chính

            modelBuilder.Entity<Voucher>()
                .Property(v => v.Code)
                .IsRequired()  // Mã voucher là bắt buộc
                .HasMaxLength(50);  // Độ dài tối đa là 50 ký tự

            modelBuilder.Entity<Voucher>()
                .Property(v => v.ReleaseDate)
                .IsRequired();  // Ngày phát hành là bắt buộc

            modelBuilder.Entity<Voucher>()
                .Property(v => v.EndDate)
                .IsRequired();  // Ngày hết hạn là bắt buộc

            
            //modelBuilder.Entity<IdentityUser>().ToTable("Users");//to change the name of table.
            base.OnModelCreating(modelBuilder);

        }
    }
}
