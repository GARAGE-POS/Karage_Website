using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    public class LandingPagesController : Controller
    {
        [Route("en/tryforfree")]
        public IActionResult TryForFree()
        {
            return View();
        }
        [Route("ar/tryforfree")]
        public IActionResult ArTryForFree()
        {
            return View();
        }
        [Route("en/mykarage")]
        public IActionResult MyKarage()
        {
            return View();
        }
        [Route("ar/mykarage")]
        public IActionResult ArMyKarage()
        {
            return View();
        }
        [Route("en/kashier")]
        public IActionResult Kashier()
        {
            return View();
        }
        [Route("ar/kashier")]
        public IActionResult ArKashier()
        {
            return View();
        }
        [Route("en/pocket")]
        public IActionResult Pocket()
        {
            return View();
        }
        [Route("ar/pocket")]
        public IActionResult ArPocket()
        {
            return View();
        }
    }
}
