using Microsoft.AspNetCore.Mvc;

namespace WebCinema.Controllers
{
    public class FAQsController : Controller
    {
        public IActionResult FAQs()
        {
            return View();
        }
    }
}
