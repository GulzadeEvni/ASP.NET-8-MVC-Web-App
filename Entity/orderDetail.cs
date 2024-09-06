using System.ComponentModel.DataAnnotations;

namespace MyApp.Entity
{
    public class orderDetail
    {
        [Key]
        public int orderDetailId { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public Orders Order { get; set; }
        public products Product { get; set; }
    }
}
