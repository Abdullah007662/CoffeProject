using System.ComponentModel.DataAnnotations;

namespace CoffeProjectWebUI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Onayı")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string? LastName { get; set; }

        [Display(Name = "Rol")]
        public string Role { get; set; } = "User";
    }
}
