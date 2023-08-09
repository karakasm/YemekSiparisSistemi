using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Admin
{
    [Authorize(Roles = Role.IS_ADMIN)]
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
                _context.Users.AddAsync(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Users/Delete/{id}")]

        public string Delete(int id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return user.Name + " " + user.Surname;
            }
            return "";
        }

        [HttpGet]
        [Route("Users/Detail/{id}")]

        public IActionResult Details(int id)
        {
            User user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
            ViewData["Roles"] = _context.Roles.ToList();
            return View("~/Views/Admin/User/Detail.cshtml", user);
        }

        [HttpPost]
        [Route("Users/Update")]
        public IActionResult Update(User user)
        {
            if(user != null)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
