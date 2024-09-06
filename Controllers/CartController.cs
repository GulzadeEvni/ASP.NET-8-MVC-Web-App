using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Entity;
using MyApp.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MyApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;


        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Sepeti al (Session veya veritabanından)
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();

            // Ürün zaten varsa, miktarı arttır
            if (cart.ContainsKey(productId))
            {
                cart[productId]++;
            }
            else
            {
                cart[productId] = 1; // Ürünü sepete ekle
            }

            // Sepeti session'a geri kaydet
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Sepetteki toplam ürün sayısını hesapla
            int cartCount = cart.Values.Sum();

            // Sepet ikonundaki sayı için geri döndür
            return Json(new { success = true, cartCount = cartCount, cartIcon = cartCount });
        }


        public IActionResult Index()
        {

            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            var productIds = cart.Keys.ToList();
            ViewBag.CartCount = cart.Values.Sum();

            var products = _context.products
                .Where(p => productIds.Contains(p.productId))
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    Description = p.description,
                    Price = p.price,
                    ImgPath = p.img_path,
                    StockQuantity = cart.ContainsKey(p.productId) ? cart[p.productId] : 0
                })
                .ToList();

            var totalPrice = products.Sum(p => p.Price * p.StockQuantity); // Calculate total price

            var viewModel = new CartViewModel
            {
                ListProduct = products,
                TotalPrice = (decimal)totalPrice
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();

            if (cart.ContainsKey(productId))
            {
                cart[productId] = Math.Max(cart[productId] + change, 0);
                if (cart[productId] == 0)
                {
                    cart.Remove(productId);
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { success = true, newQuantity = cart.ContainsKey(productId) ? cart[productId] : 0, cartCount = cart.Values.Sum() });
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();

            if (cart.ContainsKey(productId))
            {
                cart.Remove(productId);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart); // Güncellenmiş sepeti session'a kaydet

            return Json(new { success = true, cartCount = cart.Values.Sum() });
        }


        public async Task<IActionResult> CompleteShopping(decimal totalAmount)
        {
            // Kullanıcı ID'sini session'dan al
            var userId = HttpContext.Session.GetInt32("UserId");

            // Kullanıcı ID'sinin null olup olmadığını kontrol et
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı ID bulunamadı." });
            }

            // Yeni sipariş oluştur
            var order = new Orders
            {
                userId = userId.Value,  // Kullanıcı ID'sini kaydediyoruz
                orderDate = DateTime.UtcNow,
                totalAmount = totalAmount
            };

            try
            {
                // Siparişi veritabanına ekle ve kaydet
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Siparişi kaydedin ve OrderId'yi alın

                // Sepeti al (Session'dan)
                var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();

                // OrderDetail tablosuna ürünleri ekle
                foreach (var item in cart)
                {
                    var productId = item.Key;
                    var quantity = item.Value;
                    var product = await _context.products.FindAsync(productId);
                    if (product != null)
                    {
                        var orderDetail = new orderDetail
                        {
                            orderId = order.orderId,
                            productId = productId,
                            quantity = quantity,
                            price = product.price 
                        };

                        _context.orderDetail.Add(orderDetail);
                    }
                }

                await _context.SaveChangesAsync(); // OrderDetail verilerini kaydet

                // Sepeti temizle
                HttpContext.Session.Remove("Cart");

                // Başarılı sonuç ve yönlendirme URL'si döndür
                return Json(new { success = true, message = "Siparişiniz başarıyla alınmıştır.", redirectUrl = Url.Action("Index", "Home") });
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj döndür
                return Json(new { success = false, message = "Veritabanına kaydetme hatası: " + ex.Message + " İç hata: " + ex.InnerException?.Message });
            }
        }

    }

    public static class SessionExtensions  //nesneleri JSON formatında saklamak ve almak için kullanılan metodları içerir.
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);// value boş dönerse default(T) döndürülür. değilse JSON string'ini belirtilen türde (T) bir nesneye dönüştürür. 
        }
    }
}
