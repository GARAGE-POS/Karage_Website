using Microsoft.AspNetCore.Mvc;

namespace Karage_Website.Controllers
{
    public class BusinessTypeController : Controller
    {
        [Route("en/businesstype")]
        public IActionResult BusinessType()
        {
            return View();
        }
        [Route("ar/businesstype")]
        public IActionResult ArBusinessType()
        {
            return View();
        }
    }
}
