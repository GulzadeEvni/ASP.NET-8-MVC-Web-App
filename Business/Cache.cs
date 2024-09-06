using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Business
{
    public class Cache
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;

        public Cache(IMemoryCache cache, ApplicationDbContext context)
        {
            _cache = cache;
            _context = context;
        }
        public List<ProductViewModel> GetCartProducts(Dictionary<int, int> cart)
        {
            var cacheKey = "CartProducts";  // Cache'de saklanacak veriye erişmek için kullanılacak anahtar
            if (!_cache.TryGetValue(cacheKey, out List<ProductViewModel> products))// Eğer önbellekte 'cacheKey' ile eşleşen bir veri bulunmazsa,
                                                                                   // Veritabanından gerekli veriyi çek ve 'products' değişkenine ata
                                                                                   // Çekilen veriyi önbelleğe ekle.   
            {
                var productIds = cart.Keys.ToList();
                products = _context.products
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

                // Veriyi cache'e ekle
                var cacheEntryOptions = new MemoryCacheEntryOptions() //Yeni bir cache girişinin ayarlarını tanımlar.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); //Cache'deki verinin yaşam süresini ayarlar. SetSlidingExpiration, bir cache ögesinin belirli bir süre boyunca erişilmediği takdirde cache'den silinmesini sağlamak için kullanılan bir metottur.

                _cache.Set(cacheKey, products, cacheEntryOptions); //  Cache'e yeni veriyi (products) ekler ve belirlenen cacheEntryOptions ayarlarını uygular.
            }
            return products;
        }


    }
}

