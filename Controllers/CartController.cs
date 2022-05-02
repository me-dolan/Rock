using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rock.Data;
using Rock.Models;
using Rock.Models.ViewModels;
using Rock.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Rock.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart) !=null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WConst.SessionCart);
            }

            List<int> productInCart = shoppingCarts.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _context.Products.Where(u => productInCart.Contains(u.Id));

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WConst.SessionCart);
            }

            List<int> productInCart = shoppingCarts.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _context.Products.Where(u => productInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == userId),
                ProductList = productList.ToList()
            };

            return View(ProductUserVM);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WConst.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WConst.SessionCart);
            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(i => i.ProductId == id));

            List<int> productInCart = shoppingCarts.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _context.Products.Where(u => productInCart.Contains(u.Id));

            HttpContext.Session.Set(WConst.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }
    }
}
