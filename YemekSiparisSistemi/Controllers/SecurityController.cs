using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Text.Json;
using YemekSiparisSistemi.Models;

namespace YemekSiparisSistemi.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {

        private readonly FoodOrderSystemDbContext _context;

        public SecurityController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.Name.ToUpper() + " " + user.Surname.ToUpper()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName.ToLower()),
                };

                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                if (user.Role?.RoleName?.ToLower() == Role.IS_ADMIN)
                {
                    return RedirectToAction("AdminIndex", "Home");
                }
                else if (user.Role?.RoleName.ToLower() == Role.IS_CUSTOMER)
                {
                    return RedirectToAction("CustomerIndex", "Home");
                }
            }
            else
            {
                Company? company = _context.Companies.Include(c => c.Role).FirstOrDefault(c => c.Email == email && c.Password == password);

                if (company != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,Convert.ToString(company.Id)),
                        new Claim(ClaimTypes.Email, company.Email),
                        new Claim(ClaimTypes.Name, company.CompanyName),
                        new Claim(ClaimTypes.Role, company.Role?.RoleName.ToLower()),
                    };


                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("RestaurantIndex", "Home");
                }
            }
            ViewBag.LoginError = "Kullanıcı Adı veya Şifre hatalı!!!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            user.RoleId = (await _context.Roles.FirstOrDefaultAsync(role => role.RoleName.ToLower() == Role.IS_CUSTOMER)).Id;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
                {

                    new Claim(ClaimTypes.Name, user.Name.ToUpper() + " " + user.Surname.ToUpper()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName.ToLower()),
                };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("CustomerIndex", "Home");
        }
    }
}
