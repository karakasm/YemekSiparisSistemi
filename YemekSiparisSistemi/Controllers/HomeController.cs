using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers
{
    public class HomeController : Controller
    {

        private readonly FoodOrderSystemDbContext _context;


        public HomeController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }

        [AllowAnonymous]
        [Route("/")]
        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Provinces"] = _context.Provinces.ToList();
            return View("~/Views/Index.cshtml",await _context.Companies.ToListAsync<Company>());
        }

        [Authorize(Roles = Role.IS_ADMIN)]
        [Route("Admin")]
        [Route("Admin/Index")]
        [HttpGet]
        public IActionResult AdminIndex()
        {
            return View("~/Views/Admin/Index.cshtml");
        }

        [Authorize(Roles = Role.IS_RESTAURANT)]
        [Route("Restaurant")]
        [Route("Restaurant/Index")]
        [HttpGet]
        public IActionResult RestaurantIndex()
        {
            return View("~/Views/Restaurant/Index.cshtml");
        }

        [Authorize(Roles = Role.IS_CUSTOMER)]
        [Route("Customer")]
        [Route("Customer/Index")]
        [HttpGet]
        public async Task<IActionResult> CustomerIndex()
        {
            return View("~/Views/Customer/Index.cshtml", await _context.Companies.Include(c => c.Address).ThenInclude(a => a.Province).ThenInclude(a => a.Districts).ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Province/{id}/Districts")]
        public JsonResult GetAllDistrictByProvinceId(int id)
        {
            return Json(_context.Districts.Where(d => d.ProvinceId == id).ToJson());
        }
    }
}
