using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Restaurant
{
    [Authorize(Roles = Role.IS_RESTAURANT)]
    [Route("Restaurant")]
    public class MenuController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int currentCompanyId;

        public MenuController(FoodOrderSystemDbContext foodOrderSystemDbContext, IWebHostEnvironment appEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = foodOrderSystemDbContext;
            _appEnvironment = appEnvironment;
            _contextAccessor = httpContextAccessor;
            currentCompanyId = Convert.ToInt32(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        [Route("Menus")]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Restaurant/Menu/Index.cshtml", await _context.Menus.Where(m => m.CompanyId == currentCompanyId).Include(m => m.MenuProducts).ToListAsync());
        }


        [HttpGet]
        [Route("Menu/Create")]

        public IActionResult Create()
        {
            ViewData["currentCompanyId"] = currentCompanyId;
            return View("~/Views/Restaurant/Menu/Create.cshtml");
        }


        [HttpPost]
        [Route("Menu/Create")]

        public async Task<IActionResult> Create(Menu menu, List<int> ProductId, List<int> Quantity, List<int> UnitPrice)
        {
            if (menu == null || ProductId.Count == 0)
            {
                return BadRequest();
            }
            try
            {
                menu.CompanyId = currentCompanyId;
                List<MenuProduct> menuProducts = new List<MenuProduct>();
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile menuImage = Request.Form.Files["MenuImage"];
                    string fileExtension = Path.GetExtension(menuImage.FileName);

                    string fileName = menu.CompanyId.ToString() + "_" + menu.MenuName.ToLower() + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + fileExtension;

                    string path = Path.Combine(_appEnvironment.WebRootPath, "Menus\\" + menu.CompanyId.ToString());

                    string filePath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        await menuImage.CopyToAsync(fs);
                    }

                    menu.MenuImage = fileName;

                    await _context.Menus.AddAsync(menu);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < ProductId.Count; i++)
                    {
                        menuProducts.Add(new MenuProduct() { MenuId = menu.Id,
                            ProductId = ProductId[i],
                            Quantity = Convert.ToByte(Quantity[i]),
                            UnitPrice = UnitPrice[i],
                        });
                    }

                    await _context.MenuProducts.AddRangeAsync(menuProducts);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception /*ex*/)
            {
                return RedirectToAction("Index", "Menu");
            }
            return RedirectToAction("Index", "Menu");
        }
    }
}
