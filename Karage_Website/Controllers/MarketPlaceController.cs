using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    [Route("{lang:regex(^en|ar$)}/[action]")]
    public class MarketPlaceController : Controller
    {
        //[HttpGet]
        //public IActionResult MarketPlace(string lang = "en")
        //{
        //    if (lang == "ar")
        //        return View("ArMarketPlace"); // ✅ Loads Arabic view
        //    return View("MarketPlace"); // ✅ Loads English view
        //}
        public IActionResult MarketPlace()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArMarketPlace" : "MarketPlace");
        }
    }
}
