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


        [HttpGet]
        [Route("Restaurants/{restaurantId}/Products/{productId}")]

        public async Task<IActionResult> GetProduct(int? restaurantId, int? productId)
        {
            if(restaurantId == null || productId == null)
            {
                return BadRequest();
            }

            ViewData["companyInfo"] = await _context.Companies.FindAsync(restaurantId);
            Product? product = await _context.Products.FindAsync(productId);
            if(product == null || ViewData["companyInfo"] == null)
            {
                return BadRequest();
            }

            return View("~/Views/Customer/Restaurant/Product/Index.cshtml", product);
        }


        [HttpGet]
        [Route("Restaurants/{restaurantId}/Menus/{menuId}")]

        public async Task<IActionResult> GetMenu(int? restaurantId, int? menuId)
        {
            if (restaurantId == null || menuId == null)
            {
                return BadRequest();
            }

            ViewData["companyInfo"] = await _context.Companies.FindAsync(restaurantId);
            Menu? menu = await _context.Menus.Include(m=>m.MenuProducts).FirstOrDefaultAsync(m => m.Id == menuId);
            if (menu == null || ViewData["companyInfo"] == null)
            {
                return BadRequest();
            }

            return View("~/Views/Customer/Restaurant/Menu/Index.cshtml", menu);
        }
    }
}
