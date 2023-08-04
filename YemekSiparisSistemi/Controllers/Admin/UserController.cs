using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Admin
{
    [Route("Admin")]
    public class UserController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        public UserController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> Index()
        {
           return View("~/Views/Admin/User/Index.cshtml", await _context.Users.ToListAsync());
        }


        [HttpGet]
        [Route("Users/Create")]
        public string Create()
        {
            return "user create page";
        }

        [HttpPost]
        [Route("Users/Create")]
        public IActionResult Create(User user)
        {
            return View();
        }
    }
}
