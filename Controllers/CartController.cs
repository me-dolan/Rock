using Microsoft.AspNetCore.Mvc;
using Rock.Data;
using Rock.Models;
using Rock.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Rock.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

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
    }
}
