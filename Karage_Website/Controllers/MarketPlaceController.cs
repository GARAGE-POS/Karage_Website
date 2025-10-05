using Karage_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Karage_Website.Controllers
{
    [Route("{lang:regex(^en|ar$)}/[action]")]
    public class MarketPlaceController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public MarketPlaceController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
            
        }
        public IActionResult MarketPlace()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArMarketPlace" : "MarketPlace");
        }

  
        [HttpPost]
        public JsonResult SubscribeNewsLetter([FromBody] demoEmail obj)
        {
            string toEmail = _configuration["SmtpSettings:To"];
            string subject = "Newsletter Subscription at Karage";
            string message = "Query Message";

            string templatePath = Path.Combine(_env.ContentRootPath, "Template/contact.txt");
            string bodyEmail;

            bodyEmail = System.IO.File.ReadAllText(templatePath);
            bodyEmail = bodyEmail.Replace("#Date#", DateTime.Now.ToString())
                                 .Replace("#Email#", obj.Email)
                                 .Replace("#Subject#", subject)
                                 .Replace("#Message#", message);


            try
            {
                // Create the MailMessage
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_configuration["SmtpSettings:From"]),
                    Subject = subject,
                    Body = bodyEmail,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                // Configure the SMTP client
                using (SmtpClient smtp = new SmtpClient
                {
                    Host = _configuration["SmtpSettings:SmtpServer"],
                    Port = int.Parse(_configuration["SmtpSettings:SmtpPort"]),
                    Credentials = new System.Net.NetworkCredential(_configuration["SmtpSettings:UserName"], _configuration["SmtpSettings:Password"]),
                    EnableSsl = true
                })
                {
                    smtp.Send(mail);
                }

                return Json(new { success = true, message = "Thanks for joining our newsletter!" });
            }
            catch (Exception ex)
            {
         
                return Json(new { success = false, message = "Oops! Something went wrong." });
            }
        }
    }
}
