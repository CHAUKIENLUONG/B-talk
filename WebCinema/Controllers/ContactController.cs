using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace WebCinema.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
