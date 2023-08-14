using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Customer
{
    [Authorize(Roles = Role.IS_CUSTOMER)]
    [Route("Customer")]
    public class CategoryController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;

        public CategoryController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }


        [HttpGet]
        [Route("Category/{id}/Products")]

        public async Task<IActionResult> GetAllProductsByCategory(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                return PartialView("~/Views/Shared/ProductCardPartialView.cshtml", await _context.Products.Where(p => p.CategoryId == id).ToListAsync());
            }
            catch (Exception /*ex*/)
            {
                return Json(null);
            }
        }
    }
}
