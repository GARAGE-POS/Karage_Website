using Karage_Website.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Mail;
using Vitamito.Models.BLL;

namespace Karage_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
            _env = env;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("ar/home")]
        public IActionResult ArIndex()
        {
            return View();
        }
        [Route("en/about")]
        public IActionResult About()
        {
            return View();
        }
        [Route("ar/about")]
        public IActionResult ArAbout()
        {
            return View();
        }
        [Route("en/pricing")]
        public IActionResult Pricing()
        {
            return View();
        }
        [Route("ar/pricing")]
        public IActionResult ArPricing()
        {
            return View();
        }
        [Route("en/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(contactBLL obj)
        {
            ViewBag.Contact = "";
            string ToEmail, SubJect, cc, Bcc;
            cc = "";
            Bcc = "";
            ToEmail = _configuration["From"].ToString();
            SubJect = "New Query From Customer";
            string BodyEmail = System.IO.Path.Combine(_env.ContentRootPath, "Template", "contact.txt");
            DateTime dateTime = DateTime.UtcNow.Date;
            BodyEmail = BodyEmail.Replace("#Date#", dateTime.ToString("dd/MMM/yyyy"))
            .Replace("#Name#", obj.Name.ToString())
            .Replace("#Email#", obj.Email.ToString())
            .Replace("#Company#", obj.Company.ToString())
            .Replace("#Phone#", obj.Phone.ToString())
            .Replace("#Subject#", obj.Subject.ToString())
            .Replace("#Message#", obj.Message.ToString());
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(ToEmail);
                mail.From = new MailAddress(_configuration["From"].ToString());
                mail.Subject = SubJect;
                string Body = BodyEmail;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = int.Parse(_configuration["SmtpPort"].ToString());
                smtp.Host = _configuration["SmtpServer"].ToString(); //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (_configuration["From"].ToString(), _configuration["Password"].ToString());
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;

                smtp.Send(mail);
                ViewBag.Contact = "Your Query is received. Our support department contact you soon.";
            }
            catch (Exception ex)
            {
                ViewBag.Contact = "";
            }
            return View();
        }

            [Route("ar/contact")]
        public IActionResult ArContact()
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
