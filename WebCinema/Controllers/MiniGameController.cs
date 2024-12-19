using Microsoft.AspNetCore.Mvc;

namespace WebCinema.Controllers
{
    public class MiniGameController : Controller
    {
        public IActionResult MiniGame()
        {
            return View();
        }
    }
}
