using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Security.Claims;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Customer
{
    [Authorize(Roles = Role.IS_CUSTOMER)]
    [Route("Customer")]
    public class AddressController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int currentUserId;

        public AddressController(FoodOrderSystemDbContext foodOrderSystemDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = foodOrderSystemDbContext;
            _contextAccessor = httpContextAccessor;
            currentUserId = Convert.ToInt32(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        [Route("Addresses")]
        public async Task<IActionResult> Index()
        {

            return View("~/Views/Customer/Address/Index.cshtml", await _context.Users.Include(u => u.Adres).ThenInclude(a => a.Province).ThenInclude(a => a.Districts).FirstAsync(u => u.Id == currentUserId));
        }


        [HttpGet]
        [Route("Address/Create")]

        public async Task<IActionResult> Create()
        {
            ViewData["Provinces"] = await _context.Provinces.ToListAsync();
            return View("~/Views/Customer/Address/Create.cshtml");
        }

        [HttpPost]
        [Route("Address/Create")]

        public async Task<IActionResult> Create(Address? address)
        {
            if (address == null)
            {
                return BadRequest();
            }
            try
            {
                User? user = await _context.Users.FindAsync(currentUserId);
                if (user != null)
                {
                    address.Users.Add(user);
                }
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception /*ex*/)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Address");
        }

        [HttpPost]
        [Route("Address/Delete/{id}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Address? address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                try
                {
                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                    return Json(address.Tag);
                }
                catch (Exception /*ex*/)
                {
                    return Json(null);
                }
            }
            return Json(null);

        }
    }
}
