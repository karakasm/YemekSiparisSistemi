using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Customer
{
    [Authorize(Roles = Role.IS_CUSTOMER)]
    [Route("Customer")]
    public class CompanyController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        public CompanyController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }
        [HttpGet]
        [Route("Restaurants/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            ViewData["Company"] = await _context.Companies.FindAsync(id);
            return View("~/Views/Customer/Restaurant/Index.cshtml", await _context.Categories.Where(cat => cat.CompanyId == id).ToListAsync());
        }


        [HttpGet]
        [Route("Restaurants/{id}/Menus")]
        public async Task<IActionResult> GetAllMenuProducts(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            try
            {
                return PartialView("~/Views/Shared/MenuProductCardPartialView.cshtml", await _context.Menus.Where(m => m.CompanyId == id).ToListAsync());
            }catch(Exception /*ex*/)
            {
                return Json(null);
            }
        }
    }
}
