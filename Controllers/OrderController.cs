using Microsoft.AspNetCore.Mvc;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            var orderSummary = GetOrderSummary(); // Sipariş özet verilerini sağlayan metod

            return View(orderSummary);
        }

        private OrderSummaryViewModel GetOrderSummary()
        {
            // Sipariş özet bilgilerini oluşturacak bir metod
            return new OrderSummaryViewModel { };
        }
    }
}
