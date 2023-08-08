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
        public IActionResult Create()
        {
            ViewData["Provinces"] = _context.Provinces.ToList();
            ViewData["role"] = _context.Roles.FirstOrDefault(r => r.RoleName == "Restaurant");
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

            if (Request.Form.Files.Count > 0)
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
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Restaurants/Delete/{id}")]

        public string Delete(int id)
        {
            Company company = _context.Companies.Find(id);
            if(company != null)
            {
                _context.Companies.Remove(company);

                try
                {
                    // Check if file exists with its full path
                    string path = Path.Combine(_appEnvironment.WebRootPath, "Logos");
                    string filePath = Path.Combine(path, company.LogoPath);

                    if (System.IO.File.Exists(filePath))
                    {
                        // If file found, delete it
                       System.IO.File.Delete(filePath);
                    }
                }
                catch (IOException ioExp)
                {
                    Console.WriteLine(ioExp.Message);
                }

                Address companyAddress = _context.Addresses.Find(company.AddressId);

                if(companyAddress != null)
                {
                    _context.Addresses.Remove(companyAddress);
                }

                _context.SaveChanges();
                return company?.CompanyName;
            }
            return "";
        }
    }
}
