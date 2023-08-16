using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Admin
{
    [Authorize(Roles = Role.IS_ADMIN)]
    [Route("Admin")]
    public class RoleController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        public RoleController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }

        [HttpGet]
        [Route("Roles")]
        public IActionResult Index()
        {
            ViewData["restaurantCount"] = _context.Companies.Count();
            return View("~/Views/Admin/Role/Index.cshtml", _context.Roles.Include(r => r.Users).ToList());
        }

        [HttpPost]
        [Route("Roles/Create")]
        public IActionResult Create(Role role)
        {
            try
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return Json(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(null);
            }
        }



        [HttpPost]
        [Route("Roles/Delete/{id}")]
        public string Delete(int id)
        {
            Role role = _context.Roles.Find(id);
            if(role != null)
            {
                try
                {
                    _context.Roles.Remove(role);
                    _context.SaveChanges();
                    return role.RoleName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "";
                }
                
            }
            return "";
        }
    }
}