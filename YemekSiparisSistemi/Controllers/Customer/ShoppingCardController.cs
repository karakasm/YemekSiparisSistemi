using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekSiparisSistemi.Models;
using YemekSiparisSistemi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;


namespace YemekSiparisSistemi.Controllers.Customer
{
    [Authorize(Roles = Role.IS_CUSTOMER)]

    [Route("Customer")]
    public class ShoppingCardController : Controller
    {
        private readonly FoodOrderSystemDbContext _context;

        public ShoppingCardController(FoodOrderSystemDbContext foodOrderSystemDbContext)
        {
            _context = foodOrderSystemDbContext;
        }


        [HttpGet]
        [Route("Card/Index")]

        public IActionResult Index()
        {
            List<ShoppingCardItem>? card = SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card");
            ViewData["cardTotalPrice"] = card?.Sum(item => item?.Product == null ? item?.Menu?.TotalPrice * item?.Quantity : item?.Product.UnitPrice * item?.Quantity);
            return View("~/Views/Customer/Card/Index.cshtml", card);
        }


        [HttpPost]
        [Route("Card/Product/{productId}")]

        public async Task<IActionResult> AddProductToCard(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return BadRequest();
            }

            if (SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card") == null)
            {
                List<ShoppingCardItem> card = new List<ShoppingCardItem>();
                card.Add(new ShoppingCardItem() { Product = product, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", card);
            }
            else
            {
                List<ShoppingCardItem>? card = SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card");
                if (card != null)
                {
                    var item = card.FirstOrDefault(item => item?.Product?.Id == product.Id);

                    if (item == null)
                    {
                        card.Add(new ShoppingCardItem() { Product = product, Quantity = 1 });
                    }
                    else
                    {
                        item.Quantity += 1;
                    }

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", card);
                }
            }
            return RedirectToAction("Index", "ShoppingCard");
        }

        [HttpPost]
        [Route("Card/Menu/{menuId}")]

        public async Task<IActionResult> AddMenuToCard(int? menuId)
        {
            if (menuId == null)
            {
                return BadRequest();
            }

            var menu = await _context.Menus.FindAsync(menuId);

            if (menu == null)
            {
                return BadRequest();
            }

            if (SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card") == null)
            {
                List<ShoppingCardItem> card = new List<ShoppingCardItem>();
                card.Add(new ShoppingCardItem() { Menu = menu, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", card);
            }
            else
            {
                List<ShoppingCardItem>? card = SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card");
                if (card != null)
                {
                    var item = card.FirstOrDefault(item => item?.Menu?.Id == menu.Id);

                    if (item == null)
                    {
                        card.Add(new ShoppingCardItem() { Menu = menu, Quantity = 1 });
                    }
                    else
                    {
                        item.Quantity += 1;
                    }

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", card);
                }
            }
            return RedirectToAction("Index", "ShoppingCard");
        }

        [HttpPost]
        [Route("Card/Item/Delete/{itemId}")]

        public IActionResult Delete(int? itemId)
        {
            if (itemId == null)
            {
                return BadRequest();
            }

            List<ShoppingCardItem>? card = SessionHelper.GetObjectFromJson<List<ShoppingCardItem>>(HttpContext.Session, "Card");

            if(card != null)
            {
                var item = card.Find(item => item.Id == itemId);
                if(item != null)
                {
                    card.Remove(item);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", card);
                return Json(true);
            }

            return Json(null);
        }
    }
}
