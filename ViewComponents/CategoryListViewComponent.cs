using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.categories.Select(i => new CategoryViewModel()

            {
                CategoryId = i.categoryId,
                Name = i.categoryName
            }).ToListAsync();
            
            return View(categories);
        }
    }
}
