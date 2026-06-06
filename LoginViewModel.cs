using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Zadejte uživatelské jméno")]
        [Display(Name = "Uživatelské jméno")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zadejte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}