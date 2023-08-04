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
            return View("~/Views/Admin/User/Index.cshtml", await _context.Users.Include(user => user.Role).ToListAsync()); ;
        }


        [HttpGet]
        [Route("Users/Create")]
        public IActionResult Create()
        {
            ViewData["Roles"] = _context.Roles.ToList();
            return View("~/Views/Admin/User/Create.cshtml");
        }

        [HttpPost]
        [Route("Users/Create")]
        public IActionResult Create(User user)
        {
            if (user != null)
            {
                user.Role = _context.Roles.Find(user.RoleId);
                User temp = user;
                _context.Users.AddAsync(temp);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
