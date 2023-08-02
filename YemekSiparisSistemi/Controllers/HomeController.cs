using Microsoft.AspNetCore.Mvc;

namespace YemekSiparisSistemi.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("Index")]
        [HttpGet]
        public IActionResult Index()
        {
  
            return View("~/Views/Index.cshtml");
        }

        [Route("Admin")]
        [Route("Admin/Index")]
        [HttpGet]
        public IActionResult AdminIndex()
        {
            return View("~/Views/Admin/Index.cshtml");
        }

        [Route("Restaurant")]
        [Route("Restaurant/Index")]
        [HttpGet]
        public IActionResult RestaurantIndex()
        {
            return View("~/Views/Restaurant/Index.cshtml");
        }

        [Route("Customer")]
        [Route("Customer/Index")]
        [HttpGet]
        public IActionResult CustomerIndex()
        {
            return View("~/Views/Customer/Index.cshtml");
        }


    }
}
