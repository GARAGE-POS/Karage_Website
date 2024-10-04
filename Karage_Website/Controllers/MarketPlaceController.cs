using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    public class MarketPlaceController : Controller
    {
        [Route("en/marketplace")]
        public IActionResult MarketPlace()
        {
            return View();
        }
        [Route("ar/marketplace")]
        public IActionResult ArMarketPlace()
        {
            return View();
        }
    }
}
