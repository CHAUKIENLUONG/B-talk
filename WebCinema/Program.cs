using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCinema.Models;
using WebCinema.Repositories;
using WebCinema.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMovieDetailRepo, EFMovieDetailRepo>();
builder.Services.AddScoped<IMovieRepo, EFMovieRepo>();
builder.Services.AddScoped<IGenreRepo, EFGenreRepo>();
builder.Services.AddScoped<IShowtimeRepo, EFShowtimeRepo>();
builder.Services.AddScoped<IScreentimeRepo, EFScreentimeRepo>();
builder.Services.AddScoped<IRoomRepo, EFRoomRepo>();
builder.Services.AddScoped<ITicketRepo, EFTicketRepo>();

builder.Services.AddSingleton<IVnPayService, VnPayService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
