using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim gerekli.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Soy İsim gerekli.")]
        public string userLastName { get; set; }

        [Required(ErrorMessage = "E-posta gerekli.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Cinsiyet gerekli.")]
        public string gender { get; set; }
    }
}
