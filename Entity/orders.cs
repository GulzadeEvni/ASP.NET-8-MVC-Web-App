using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Entity
{
    [Table("orders")]
    public class Orders
    {
        [Key]
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        public int userId { get; set; }
        public decimal totalAmount { get; set; }
    }
}
