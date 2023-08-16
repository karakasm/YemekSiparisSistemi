using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Customer
{

    [Authorize(Roles = Role.IS_CUSTOMER)]
    [Route("Customer")]
    public class DistrictController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;
        public DistrictController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }

        [HttpGet]
        [Route("Province/{id}/Districts")]
        public JsonResult GetAllDistrictByProvinceId(int id)
        {
            return Json(_context.Districts.Where(d => d.ProvinceId == id).ToJson());
        }
    }
}
