using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    [Route("{lang:regex(^en|ar$)}/[action]")]
    public class BusinessTypeController : Controller
    {
        public IActionResult BusinessType()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArBusinessType" : "BusinessType");
        }
        //[Route("ar/businesstype")]
        //public IActionResult ArBusinessType()
        //{
        //    return View();
        //}
    }
}
