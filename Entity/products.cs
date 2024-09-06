using System.ComponentModel.DataAnnotations;

namespace MyApp.Entity
{
    public class products
    {
        [Key]
        public int productId { get; set; }
        [Required]
        public string productName { get; set; }

        [Required]
        public string description { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int? stockQuantity { get; set; }
        [Required]
        public int categoryId { get; set; }
        public string img_path { get; set; }
    }
}
