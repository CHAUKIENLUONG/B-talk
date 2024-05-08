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

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string name = form["name"];
            string email = form["email"];
            string subject = form["subject"];
            string message = form["message"];

            // Send email using ASP.NET
            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add("luong211003@gmail.com");
                mail.From = new MailAddress(email);
                mail.Subject = subject;
                mail.Body = message;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Send(mail);
                }
            }

            return Json(new { success = true, message = "Message sent successfully!" });
        }
    }
}
