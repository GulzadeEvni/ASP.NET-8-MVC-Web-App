using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _context.products
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    Price = p.price,
                    Description = p.description,
                    ImgPath = p.img_path,
                    StockQuantity = p.stockQuantity
                }).ToListAsync();
            return View(cards);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _context.products
                .Where(p => p.productId == id)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    Price = p.price,
                    Description = p.description,
                    ImgPath = p.img_path,
                    StockQuantity = p.stockQuantity
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            ViewData["productId"] = id;

            return View(product);
        }

    }
}
