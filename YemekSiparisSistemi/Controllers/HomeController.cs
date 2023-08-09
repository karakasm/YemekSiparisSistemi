using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YemekSiparisSistemi.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("/")]
        [Route("Index")]
        [HttpGet]
        public IActionResult Index()
        {
  
            return View("~/Views/Index.cshtml");
        }

        [Authorize(Roles ="Admin")]
        [Route("Admin")]
        [Route("Admin/Index")]
        [HttpGet]
        public IActionResult AdminIndex()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.ValueType == "Email");
            return View("~/Views/Admin/Index.cshtml");
        }

        [Authorize(Roles ="Restaurant")]
        [Route("Restaurant")]
        [Route("Restaurant/Index")]
        [HttpGet]
        public IActionResult RestaurantIndex()
        {
            return View("~/Views/Restaurant/Index.cshtml");
        }

        [Authorize(Roles = "Customer")]
        [Route("Customer")]
        [Route("Customer/Index")]
        [HttpGet]
        public IActionResult CustomerIndex()
        {
            return View("~/Views/Customer/Index.cshtml");
        }


    }
}
