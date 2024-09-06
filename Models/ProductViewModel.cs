namespace MyApp.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public int? StockQuantity { get; set; }
    }
}