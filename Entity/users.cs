using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Entity
{
    public class users
    {
        [Key]
        public int userId { get; set; }
        public string userName { get; set; }
        public string userLastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
    }
}
