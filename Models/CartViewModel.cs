namespace MyApp.Models
{
    public class CartViewModel
    {
        public List<ProductViewModel> ListProduct { get; set; }
        public decimal TotalPrice { get; set; } = 0;
    }
}
