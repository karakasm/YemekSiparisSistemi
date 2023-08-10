using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Restaurant
{
    [Authorize(Roles = Role.IS_RESTAURANT)]
    [Route("Restaurant")]
    public class CategoryController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int currentCompanyId;

        public CategoryController(FoodOrderSystemDbContext foodOrderSystemDbContext,IHttpContextAccessor httpContextAccessor)
        {
            _context = foodOrderSystemDbContext;
            _contextAccessor = httpContextAccessor;
            currentCompanyId = Convert.ToInt32(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        
        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Restaurant/Category/Index.cshtml", await _context.Categories.Where(c => c.CompanyId == currentCompanyId).ToListAsync());
        }

        [HttpPost]
        [Route("Category/Create")]

        public async Task<IActionResult> Create([Bind(include:"CategoryName")]Category category)
        {
            if(category == null)
            {

                return BadRequest();
            }
            
            
            try
            {
                category.CompanyId = currentCompanyId;
                if (ModelState.IsValid)
                {
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();
                    return Json(category);
                }

            }
            catch(DataException /*dex*/)
            {
                
            }
            return Json(null);
        }


        [HttpPost]
        [Route("Category/Delete/{id}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            Category? category = _context.Categories.Find(id);

            if(category != null)
            {
                try
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    return Json(category.CategoryName);
                }
                catch (Exception /*ex*/)
                {

                }
            }

            return Json(null);
        }
    }
}
