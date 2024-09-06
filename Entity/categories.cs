using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Entity
{
    //[Table("categories")]
    public class categories
    {
        [Key]
        public int categoryId { get; set; }

        [Required]
        public string categoryName { get; set; }
      
    }
}
