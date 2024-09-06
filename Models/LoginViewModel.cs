using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi girilmesi gerekli.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Şifre girilmesi gerekli.")]
        [DataType(DataType.Password)]
        public string password { get; set; }



    }
}
