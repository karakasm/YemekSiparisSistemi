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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int currentUserId;

        public HomeController(FoodOrderSystemDbContext foodOrderSystemDbContext,IHttpContextAccessor httpContextAccessor)
        {
            _context = foodOrderSystemDbContext;
            _contextAccessor = httpContextAccessor;
            currentUserId = Convert.ToInt32(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

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
            ViewData["rolCount"] = _context.Roles.Count<Role>();
            ViewData["userCount"] = _context.Users.Count<User>();
            ViewData["restaurantCount"] = _context.Companies.Count<Company>();
            ViewData["orderCount"] = _context.Orders.Count<Order>();
            ViewData["deliveryCount"] = _context.Deliveries.Count<Delivery>();
            return View("~/Views/Admin/Index.cshtml");
        }

        [Authorize(Roles = Role.IS_RESTAURANT)]
        [Route("Restaurant")]
        [Route("Restaurant/Index")]
        [HttpGet]
        public IActionResult RestaurantIndex()
        {

            ViewData["categoryCount"] = _context.Categories.Where(c => c.CompanyId == currentUserId).Count<Category>();
            ViewData["productCount"] = _context.Products.Where(p => p.CompanyId == currentUserId).Count<Product>();
            ViewData["menuCount"] = _context.Menus.Where(m => m.CompanyId == currentUserId).Count<Menu>();
            ViewData["orderCount"] = _context.Orders.Where(o => o.CompanyId == currentUserId).Count<Order>();
            ViewData["deliveryCount"] = _context.Deliveries.Where(d => d.CompanyId == currentUserId).Count<Delivery>();
            ViewData["courierCount"] = _context.Couriers.Where(c => c.CompanyId == currentUserId).Count<Courier>();

            return View("~/Views/Restaurant/Index.cshtml");
        }

        [Authorize(Roles = Role.IS_CUSTOMER)]
        [Route("Customer")]
        [Route("Customer/Index")]
        [HttpGet]
        public async Task<IActionResult> CustomerIndex()
        {
            return View("~/Views/Customer/Index.cshtml", await _context.Companies.Include(c => c.Address).ThenInclude(a => a != null ? a.Province : null).ThenInclude(a => a != null ? a.Districts : null).ToListAsync());
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
