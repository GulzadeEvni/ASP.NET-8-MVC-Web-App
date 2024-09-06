using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: ApplicationDbContext'i dependency injection ile alır
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Veritabanından kategorileri çekip view'a gönderir
        public async Task<IActionResult> ProductsByCategory(int id)
        {
            var products = await _context.products
                .Where(p => p.categoryId == id)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    Description = p.description,
                    Price = p.price,
                    ImgPath =p.img_path
                })
                .ToListAsync();

            return View(products);
        }

    }
}
