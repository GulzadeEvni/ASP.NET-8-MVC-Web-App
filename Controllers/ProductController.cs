using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<IActionResult> ProductsByCategory(int categoryId)
        {
            var products = await _context.products
                .Where(p => p.categoryId == categoryId)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    Description = p.description,
                    Price = p.price
                })
                .ToListAsync();

            return View(products);
        }

    }
}
