using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var _book = getAllBook();
            ViewBag.book = _book;
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //GET ALL PRODUCT
        public List<Book> getAllBook()
        {
            return _context.Book.ToList();
        }

        //GET DETAIL PRODUCT
        public Book getDetailProduct(int id)
        {
            var book = _context.Book.Find(id);
            return book;
        }

        //ADD CART
        public IActionResult addCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart"); //get key cart
            if (cart == null)
            {
                var book = getDetailProduct(id);
                List<Cart> listCart = new List<Cart>()
                {
                    new Cart
                    {
                        Book = book,
                        Quantity = 1
                    }
                };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));

            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                }

                if (check)
                {
                    dataCart.Add(new Cart
                    {
                        Book = getDetailProduct(id),
                        Quantity = 1
                    });
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                // var cart2 = HttpContext.Session.GetString("cart");//get key cart
                //  return Json(cart2);
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult ListCart()
        {
            var cart = HttpContext.Session.GetString("cart"); //get key cart
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult updateCart(int id, int quantity)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (quantity > 0)
                {
                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].Book.Id == id)
                        {
                            dataCart[i].Quantity = quantity;
                        }
                    }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                }

                var cart2 = HttpContext.Session.GetString("cart");
                return Ok(quantity);
            }

            return BadRequest();

        }

        public IActionResult deleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return RedirectToAction(nameof(ListCart));
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            List <Category> categories = new List<Category>();
            var model=_context.Set<Book>().Include(c=>c.Category).FirstOrDefault(m => m.Id == id);
            return View(model);
        }
    }
}
