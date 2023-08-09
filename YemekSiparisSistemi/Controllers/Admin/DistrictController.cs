using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Data;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
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
