using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Admin
{
    [Route("Admin")]
    public class RestaurantController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;

        private readonly FoodOrderSystemDbContext _context;

        public RestaurantController(FoodOrderSystemDbContext foodOrderSystemDbContext, IWebHostEnvironment appEnvironment)
        {
            _context = foodOrderSystemDbContext;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        [Route("Restaurants")]
        public IActionResult Index()
        {
            
            return View("~/Views/Admin/Restaurant/Index.cshtml", _context.Companies.Include(c => c.Address).ToList());
        }

        [HttpGet]
        [Route("Restaurants/Create")]
        public IActionResult Create() {
            ViewData["Provinces"] = _context.Provinces.ToList();
           // ViewData["Districts"] = _context.Districts.ToList();
            return View("~/Views/Admin/Restaurant/Create.cshtml");
        }

        [HttpPost]
        [Route("Restaurants/Create")]

        public async Task<IActionResult> Create(Company company, Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            company.AddressId = address.Id;

            if(Request.Form.Files.Count > 0)
            {
                try
                {
                    IFormFile? logo = Request.Form.Files["Logo"];

                    string fileExtension = Path.GetExtension(logo.FileName);

                    string fileName = company.AddressId.ToString() + "_" + company.CompanyName?.ToLower() + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + fileExtension;

                    string path = Path.Combine(_appEnvironment.WebRootPath, "Logos");


                    string filePath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                       await logo.CopyToAsync(fs);
                    }

                    company.LogoPath = fileName;

                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.Write(ex);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
