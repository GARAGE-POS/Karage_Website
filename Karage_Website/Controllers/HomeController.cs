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
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;
        }


        [Route("{lang:regex(^en|ar$)}/home")]
        public IActionResult Index(string lang)
        {
            if (lang == "ar")
                return View("ArIndex");
            return View("Index");
        }

        [Route("{lang:regex(^en|ar$)}/about")]
        public IActionResult About(string lang)
        {
            if (lang == "ar")
                return View("ArAbout");
            return View("About");
        }

        [Route("{lang:regex(^en|ar$)}/pricing")]
        public IActionResult Pricing(string lang)
        {
            if (lang == "ar")
                return View("ArPricing");
            return View("Pricing");
        }

        [Route("{lang:regex(^en|ar$)}/contact")]
        public IActionResult Contact(string lang)
        {
            if (lang == "ar")
                return View("ArContact");
            return View("Contact");
        }



        [HttpPost]
        [Route("{lang:regex(^en|ar$)}/contact")]
        public JsonResult Contact(string lang, [FromBody] contactBLL obj)
        {
            string ToEmail = _configuration["SmtpSettings:To"];
            string SubJect = "New Query From Customer";
            string BodyEmail = Path.Combine(_env.ContentRootPath, "Template", "contact.txt");
            string emailTemplate = System.IO.File.ReadAllText(BodyEmail);

            emailTemplate = emailTemplate.Replace("#Date#", DateTime.UtcNow.ToString("dd/MMM/yyyy"))
                .Replace("#Name#", obj.Name ?? "")
                .Replace("#Email#", obj.Email ?? "")
                .Replace("#Company#", obj.Company ?? "")
                .Replace("#Phone#", obj.Phone ?? "")
                .Replace("#Subject#", obj.Subject ?? "")
                .Replace("#Message#", obj.Message ?? "");

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(ToEmail);
                mail.From = new MailAddress(_configuration["SmtpSettings:From"]);
                mail.Subject = SubJect;
                mail.Body = emailTemplate;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Port = int.Parse(_configuration["SmtpSettings:SmtpPort"]),
                    Host = _configuration["SmtpSettings:SmtpServer"],
                    Credentials = new NetworkCredential(
                        _configuration["SmtpSettings:From"],
                        _configuration["SmtpSettings:Password"]
                    ),
                    EnableSsl = true
                };

                smtp.Send(mail);

                return Json(new { success = true, message = "Your query is received. Our support team will contact you soon." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "There was a problem sending your message. Please try again later." });
            }
        }





        [HttpPost]
        [Route("Home/SubmitTrialForm")]
        public async Task<IActionResult> SubmitTrialForm([FromQuery] string lang, [FromBody] ZohoModel model)
        {
            bool isArabicPage = lang == "ar";


            using var httpClient = new HttpClient();
            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["SingleLine"] = model.FullName ?? "N/A",
                ["SingleLine2"] = model.Company ?? "N/A",
                ["Email"] = model.Email ?? "test@example.com",
                ["PhoneNumber_countrycodeval"] = model.PhoneCode ?? "",
                ["PhoneNumber_countrycode"] = model.PhoneNumber ?? "",
                ["Address_City"] = model.City ?? "",
                ["Address_Country"] = model.Country ?? "",
                ["Dropdown"] = model.BusinessType ?? "",
                ["Dropdown1"] = model.PrefilledProducts ?? ""
            });


            var response = await httpClient.PostAsync(
                "https://forms.zohopublic.sa/karage1/form/KaragePosFreeTrailForm/formperma/BkEJiKh49zfwlm8Wu7YW_uesZdCIG40jWnPz4so5WFw/htmlRecords/submit",
                content
            );

            var result = await response.Content.ReadAsStringAsync();

            if (result.Contains("Duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { success = false, message = "This email is already registered" });
            }

            if (result.Contains("country code", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { success = false, message = "Please Enter a Valid Country Code" });
            }

            if (result.Contains("ThankYou", StringComparison.OrdinalIgnoreCase))
            {
              
                try
                {
                    string toEmail = _configuration["SmtpSettings:To"];
                    string bodyEmailPath = System.IO.Path.Combine(_env.ContentRootPath, "Template", "contact.txt");
                    string emailTemplate = System.IO.File.ReadAllText(bodyEmailPath);

                    emailTemplate = emailTemplate.Replace("#Date#", DateTime.UtcNow.ToString("dd/MMM/yyyy"))
                        .Replace("#Name#", model.FullName ?? "")
                        .Replace("#Email#", model.Email ?? "")
                        .Replace("#Phone#", model.PhoneNumber ?? "")
                        .Replace("#Subject#", "New Inquiries at Karage")
                        .Replace("#Message#", "Query Message");

                    using var mail = new MailMessage
                    {
                        From = new MailAddress(_configuration["SmtpSettings:From"]),
                        Subject = "New Query From Customer",
                        Body = emailTemplate,
                        IsBodyHtml = true
                    };

                    mail.To.Add(toEmail);

                    using var smtp = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Port = int.Parse(_configuration["SmtpSettings:SmtpPort"]),
                        Host = _configuration["SmtpSettings:SmtpServer"],
                        Credentials = new NetworkCredential(
                            _configuration["SmtpSettings:From"],
                            _configuration["SmtpSettings:Password"]
                        ),
                        EnableSsl = true
                    };

                    smtp.Send(mail);

                    return Json(new
                    {
                        success = true,
                        message = isArabicPage
                       ? "شكرًا لك! تم إرسال طلبك للتجربة المجانية بنجاح، سنتواصل معك قريباً."
                       : "Thank you! Your free trial request has been submitted successfully. We’ll contact you shortly."
                    });
                }
                catch (Exception)
                {

                    //for now we do that because we dont have offical credntials for email sending
                    return Json(new
                    {
                        success = true,
                        message = isArabicPage
                     ? "شكرًا لك! تم إرسال طلبك للتجربة المجانية بنجاح، سنتواصل معك قريباً."
                     : "Thank you! Your free trial request has been submitted successfully. We’ll contact you shortly."
                    });
                }
            }

            return Json(new { success = false, message = "Zoho form submission failed" });
        }



        [Route("Home/Subscribe")]
        [HttpPost]
        public JsonResult Subscribe([FromBody] demoEmail obj)
        {
            string toEmail = _configuration["SmtpSettings:To"];
            string subject = "New Subscription at Karage";
            string message = "Query Message";

            // Read and customize the email template
            string templatePath = Path.Combine(_env.ContentRootPath, "Template/contact.txt");
            string bodyEmail;

            bodyEmail = System.IO.File.ReadAllText(templatePath);
            bodyEmail = bodyEmail.Replace("#Date#", DateTime.Now.ToString())
                                 .Replace("#Name#", obj.Name)
                                 .Replace("#Email#", obj.Email)
                                 .Replace("#Phone#", obj.Phone)
                                 .Replace("#Subject#", subject)
                                 .Replace("#Message#", message);
            //.Replace("#company#", obj.Company)
            //.Replace("#companysize#", obj.CompanySize)
            //.Replace("#position#", obj.Position);


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
                    smtp.Send(mail); // Send the email
                    // var whatsappResult = WhatsAppSend(obj);
                    //var api = new OdooLeadApi();
                    //api.CreateLead("New Lead", obj.Name, obj.Email, obj.Phone);
                }

                return Json(new { success = true, message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                // Log error and return failure message
                return Json(new { success = false,  message = "Oops! Something went wrong." });
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
