using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YemekSiparisSistemi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YemekSiparisSistemi.Controllers.Restaurant
{
    [Authorize(Roles = Role.IS_RESTAURANT)]
    [Route("Restaurant")]
    public class ProductController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int currentCompanyId;
        public ProductController(FoodOrderSystemDbContext foodOrderSystemDbContext, IWebHostEnvironment appEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = foodOrderSystemDbContext;
            _appEnvironment = appEnvironment;
            _contextAccessor = httpContextAccessor;
            currentCompanyId = Convert.ToInt32(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        [Route("Products")]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Restaurant/Product/Index.cshtml", await _context.Products.Where(p => p.CompanyId == currentCompanyId).Include(p => p.Category).ToListAsync());
        }

        [HttpGet]
        [Route("Products/Create")]
        public async Task<IActionResult> Create()
        {
            return View("~/Views/Restaurant/Product/Create.cshtml", await _context.Categories.Where(c => c.CompanyId == currentCompanyId).ToListAsync());
        }

        [HttpPost]
        [Route("Products/Create")]

        public async Task<IActionResult> Create(Product? product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            try
            {
                product.CompanyId = currentCompanyId;
                if(Request.Form.Files.Count > 0)
                {
                    IFormFile productImage = Request.Form.Files["ProductImagePath"];
                    string fileExtension = Path.GetExtension(productImage.FileName);

                    string fileName = product.CategoryId.ToString() + "_" + product.ProductName.ToLower() + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + fileExtension;

                    string path = Path.Combine(_appEnvironment.WebRootPath, "Products\\" + product.CompanyId.ToString());

                    string filePath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        await productImage.CopyToAsync(fs);
                    }

                    product.ProductImagePath = fileName;

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception /*ex*/)
            {
                return RedirectToAction("Index", "Product");
            }
            return RedirectToAction("Index","Product");
        }

        [HttpPost]
        [Route("Products/Delete/{id}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            try
            {
                Product? product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return BadRequest();
                }
                string path = Path.Combine(_appEnvironment.WebRootPath, "Products\\" + product.CompanyId.ToString());

                string filePath = Path.Combine(path, product.ProductImagePath);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it
                    System.IO.File.Delete(filePath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Json(product.ProductName);

            }
            catch(Exception /*ex*/)
            {
                return Json(null);
            }
        }

        [HttpGet]
        [Route("{id}/Products")]

        public async Task<IActionResult> GetAllProductsByCompanyId(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            return Json(await _context.Products.Where(p => p.CompanyId == id).ToListAsync());
        }
    }
}
