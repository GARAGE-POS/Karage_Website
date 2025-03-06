//using Microsoft.AspNetCore.Mvc;

//namespace Karage_Website.Controllers
//{
//    public class LandingPagesController : Controller
//    {
//        [Route("{lang:regex(^en|ar$)}/tryforfree")]
//        public IActionResult TryForFree(string lang)
//        {
//            if (lang == "ar")
//                return View("ArTryForFree");
//            return View("TryForFree");
//        }
//        //[Route("en/tryforfree")]
//        //public IActionResult TryForFree()
//        //{
//        //    return View();
//        //}
//        //[Route("ar/tryforfree")]
//        //public IActionResult ArTryForFree()
//        //{
//        //    return View();
//        //}
//        [Route("{lang:regex(^en|ar$)}/mykarage")]
//        public IActionResult MyKarage(string lang)
//        {
//            if (lang == "ar")
//                return View("ArMyKarage");
//            return View("MyKarage");
//        }
//        //[Route("en/mykarage")]
//        //public IActionResult MyKarage()
//        //{
//        //    return View();
//        //}
//        //[Route("ar/mykarage")]
//        //public IActionResult ArMyKarage()
//        //{
//        //    return View();
//        //}
//        [Route("{lang:regex(^en|ar$)}/kashier")]
//        public IActionResult Kashier(string lang)
//        {
//            if (lang == "ar")
//                return View("ArKashier");
//            return View("Kashier");
//        }
//        //[Route("en/kashier")]
//        //public IActionResult Kashier()
//        //{
//        //    return View();
//        //}
//        //[Route("ar/kashier")]
//        //public IActionResult ArKashier()
//        //{
//        //    return View();
//        //}
//        [Route("{lang:regex(^en|ar$)}/pocket")]
//        public IActionResult Pocket(string lang)
//        {
//            if (lang == "ar")
//                return View("ArPocket");
//            return View("Pocket");
//        }
//        //[Route("en/pocket")]
//        //public IActionResult Pocket()
//        //{
//        //    return View();
//        //}
//        //[Route("ar/pocket")]
//        //public IActionResult ArPocket()
//        //{
//        //    return View();
//        //}
//        [Route("{lang:regex(^en|ar$)}/TermsandCondition")]
//        public IActionResult Terms(string lang)
//        {
//            if (lang == "ar")
//                return View("ArTerms");
//            return View("Terms");
//        }
//        //[Route("en/TermsandCondition")]
//        //public IActionResult Terms()
//        //{
//        //    return View();
//        //}
//        //[Route("ar/TermsandCondition")]
//        //public IActionResult ArTerms()
//        //{
//        //    return View();
//        //}
//    }
//}
using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    [Route("{lang:regex(^en|ar$)}/[action]")]
    public class LandingPagesController : Controller
    {
        public IActionResult TryForFree()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArTryForFree" : "TryForFree");
        }

        public IActionResult Kashier()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArKashier" : "Kashier");
        }

        public IActionResult TermsAndConditions()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArTerms" : "Terms");
        }

        public IActionResult Pocket()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArPocket" : "Pocket");
        }

        public IActionResult MyKarage()
        {
            string lang = RouteData.Values["lang"]?.ToString() ?? "en";
            return View(lang == "ar" ? "ArMyKarage" : "MyKarage");
        }
    }
}
    
