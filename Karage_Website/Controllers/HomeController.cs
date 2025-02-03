using Karage_Website.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Diagnostics;
using System.Net.Mail;
using Vitamito.Models.BLL;
using System.Net;
using System.Collections.Generic; // For Dictionary
using CookComputing.XmlRpc;

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
        //[HttpGet]
        //public JsonResult Subscribe(demoEmail obj)
        //{
        //    string toEmail = _smtpSettings.From; // Send to the 'From' email from settings
        //    string subject = "New Subscription at Karage";
        //    string cc = string.Empty;
        //    string bcc = string.Empty;

        //    // Read the email template
        //    //string bodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/Template/newsletter.txt"));
        //    //bodyEmail = bodyEmail.Replace("#email#", obj.Email);

        //    string templatePath = Path.Combine(_env.ContentRootPath, "Template/contact.txt");
        //    string bodyEmail = System.IO.File.ReadAllText(templatePath);
        //    bodyEmail = bodyEmail.Replace("#name#", obj.Name);
        //    bodyEmail = bodyEmail.Replace("#email#", obj.Email);
        //    bodyEmail = bodyEmail.Replace("#phone#", obj.Phone);
        //    bodyEmail = bodyEmail.Replace("#company#", obj.Company);
        //    bodyEmail = bodyEmail.Replace("#companysize#", obj.CompanySize);
        //    bodyEmail = bodyEmail.Replace("#position#", obj.Position);

        //    try
        //    {
        //        // Create the MailMessage
        //        MailMessage mail = new MailMessage
        //        {
        //            From = new MailAddress(_smtpSettings.From),
        //            Subject = subject,
        //            Body = bodyEmail,  // Set the email body
        //            IsBodyHtml = true
        //        };
        //        mail.To.Add(toEmail);

        //        // Configure the SMTP client
        //        SmtpClient smtp = new SmtpClient
        //        {
        //            Host = _smtpSettings.SmtpServer,
        //            Port = _smtpSettings.SmtpPort,
        //            Credentials = new System.Net.NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
        //            EnableSsl = true
        //        };

        //        // Send the email
        //        smtp.Send(mail);
        //        ViewBag.Status = "Email sent successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Status = $"Error: {ex.Message}";
        //    }

        //    return Json(obj);
        //}
        [Route("Home/Subscribe")]
        [HttpPost]
        public JsonResult Subscribe(demoEmail obj)
        {
            string toEmail = _smtpSettings.From; // Send to the 'From' email from settings
            string subject = "New Subscription at Karage";

            // Read and customize the email template
            string templatePath = Path.Combine(_env.ContentRootPath, "Template/contact.txt");
            string bodyEmail;
            try
            {
                bodyEmail = System.IO.File.ReadAllText(templatePath);
                bodyEmail = bodyEmail.Replace("#name#", obj.Name)
                                     .Replace("#email#", obj.Email)
                                     .Replace("#phone#", obj.Phone)
                                     .Replace("#company#", obj.Company)
                                     .Replace("#companysize#", obj.CompanySize)
                                     .Replace("#position#", obj.Position);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error loading template: {ex.Message}" });
            }
            try
            {
                // Create the MailMessage
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.From),
                    Subject = subject,
                    Body = bodyEmail,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                // Configure the SMTP client
                using (SmtpClient smtp = new SmtpClient
                {
                    Host = _smtpSettings.SmtpServer,
                    Port = _smtpSettings.SmtpPort,
                    Credentials = new System.Net.NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                    EnableSsl = true
                })
                {
                    //smtp.Send(mail); // Send the email
                    // var whatsappResult = WhatsAppSend(obj);
                    var api = new OdooLeadApi();
                    api.CreateLead("New Lead", obj.Name, obj.Email, obj.Phone);
                }

                return Json(new { success = true, message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                // Log error and return failure message
                return Json(new { success = false, url = "/Home/ar/home", message = $"Oops! Something went wrong." });
            }
        }
        public interface IOdooXmlRpc : IXmlRpcProxy
        {
            [XmlRpcMethod("authenticate")]
            object Authenticate(
                string db,
                string username,
                string password,
                object[] args = null);

            [XmlRpcMethod("execute_kw")]
            object Execute(
                string db,
                int userId,
                string password,
                string model,
                string method,
                object[] args,
                IDictionary<string, object> kwargs = null);
        }

        public class OdooLeadApi
        {
            private const string OdooUrlCommon = "https://garagepos.odoo.com/xmlrpc/2/common";
            private const string OdooUrlObject = "https://garagepos.odoo.com/xmlrpc/2/object";
            private const string Db = "garagepos"; // Your database name
            private const string Username = "Shariqmalik@garage.sa";
            private const string Password = "sharkgarage123@";

            private readonly IOdooXmlRpc _commonClient;
            private readonly IOdooXmlRpc _objectClient;
            private int _userId;

            public OdooLeadApi()
            {
                // Initialize the XML-RPC clients
                _commonClient = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                _commonClient.Url = OdooUrlCommon;

                _objectClient = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                _objectClient.Url = OdooUrlObject;

                Authenticate();
            }

            private void Authenticate()
            {
                // Authenticate and retrieve the user ID
                var userId = _commonClient.Authenticate(Db, Username, Password);

                if (userId == null || !(userId is int))
                {
                    throw new Exception("Authentication failed: Invalid username or password.");
                }

                _userId = (int)userId;
                Console.WriteLine($"Authenticated successfully with user ID: {_userId}");
            }


            public void CreateLead(string leadName, string contactName, string email, string phone)
            {
                // Define the lead data
                var leadData = new Dictionary<string, object>
                {
                    { "name", leadName },
                    { "contact_name", contactName },
                    { "email_from", email },
                    { "phone", phone }
                };

                // Ensure all parameters are correctly passed
                var args = new object[] { leadData };

                try
                {
                    // Call the create method on the 'crm.lead' model
                    int leadId = (int)_objectClient.Execute(
                        Db, _userId, Password, "crm.lead", "create", args);

                    Console.WriteLine($"Lead created successfully with ID: {leadId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating lead: {ex.Message}");
                    throw;
                }
            }

        }

        [HttpPost]
        public ActionResult WhatsAppSend(demoEmail model)
        {
            try
            {
                var Name = model.Name;
                var Email = model.Email;
                var Phone = model.Phone;
                var Company = model.Company;
                var CompanySize = model.CompanySize;
                var Position = model.Position;

                string apiUrl = "https://graph.facebook.com/v16.0/104897916024556/messages";
                string accessToken = "Bearer EAAXW0hLwIIwBOzNXZCdaaeqg7fdsBZB8IWeb7Dl2hh5qWBPvZCxpyC0lbKEJvafy6gKF3dvep9nhzHxukBooe5GZBg6p6BNZB6BAlZBuJK7TMsu4kpMNWQcjhYZBS7DcuANJcZA7F9XiHZAwngMdFWKYSuk8g0BbA7qpgsZAUu9y0Unz2RceZAaUfUmUOJfKFrSpRKAJ9ZCMZASP54AxO7aZAH";

                string jsonBody = "{" +
                    "\"messaging_product\": \"whatsapp\"," +
                    "\"recipient_type\": \"individual\"," +
                    "\"to\": \"" + Phone + "\"," +
                    "\"type\": \"template\"," +
                    "\"template\": {" +
                    "    \"name\": \"variable_template\"," +
                    "    \"language\": {" +
                    "        \"code\": \"ar\"" +
                    "    }," +
                    "    \"components\": [" +
                    "        {" +
                    "            \"type\": \"header\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + Name + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "        {" +
                    "            \"type\": \"body\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + Email + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "       {" +
                    "            \"type\": \"body\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + Phone + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "       {" +
                    "            \"type\": \"body\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + Company + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "       {" +
                    "            \"type\": \"body\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + CompanySize + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "       {" +
                    "            \"type\": \"body\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"" + Position + "\"" +
                    "                }" +
                    "            ]" +
                    "        }," +
                    "        {" +
                    "            \"type\": \"button\"," +
                    "            \"sub_type\": \"url\"," +
                    "            \"index\": \"0\"," +
                    "            \"parameters\": [" +
                    "                {" +
                    "                    \"type\": \"text\"," +
                    "                    \"text\": \"home\"" +
                    "                }" +
                    "            ]" +
                    "        }" +
                    "    ]" +
                    "}}";

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("Authorization", accessToken);

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonBody);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseData = streamReader.ReadToEnd();

                    }
                    return View();
                }
                catch (WebException ex)
                {
                    // Handle any exceptions here
                    Console.WriteLine(ex.Message);
                    return View();
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");
        }
    }
}
