using Karage_Website.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Mail;

namespace Karage_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public JsonResult Subscribe(demoEmail obj)
        {
            string toEmail = _smtpSettings.From; // Send to the 'From' email from settings
            string subject = "New Subscription at KarachiFlora";
            string cc = string.Empty;
            string bcc = string.Empty;

            // Read the email template
            //string bodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/Template/newsletter.txt"));
            //bodyEmail = bodyEmail.Replace("#email#", obj.Email);

            string templatePath = Path.Combine(_env.ContentRootPath, "Template/newsletter.txt");
            string bodyEmail = System.IO.File.ReadAllText(templatePath);
            bodyEmail = bodyEmail.Replace("#name#", obj.Name);
            bodyEmail = bodyEmail.Replace("#email#", obj.Email);
            bodyEmail = bodyEmail.Replace("#phone#", obj.Phone);
            bodyEmail = bodyEmail.Replace("#company#", obj.Company);
            bodyEmail = bodyEmail.Replace("#companysize#", obj.CompanySize);
            bodyEmail = bodyEmail.Replace("#position#", obj.Position);

            try
            {
                // Create the MailMessage
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.From),
                    Subject = subject,
                    Body = bodyEmail,  // Set the email body
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                // Configure the SMTP client
                SmtpClient smtp = new SmtpClient
                {
                    Host = _smtpSettings.SmtpServer,
                    Port = _smtpSettings.SmtpPort,
                    Credentials = new System.Net.NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                    EnableSsl = true
                };

                // Send the email
                smtp.Send(mail);
                ViewBag.Status = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Status = $"Error: {ex.Message}";
            }

            return Json(obj);
        }

    }
}
