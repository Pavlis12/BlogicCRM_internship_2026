using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class AdvisorFormViewModel
    {
        [Required(ErrorMessage = "Jméno poradce je povinné.")]
        [Display(Name = "Jméno")]
        public string Jmeno { get; set; }

        [Required(ErrorMessage = "Příjmení poradce je povinné.")]
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; }

        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon je povinný.")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Rodné číslo je povinné.")]
        [Display(Name = "Rodné číslo")]
        public string RodneCislo { get; set; }

        [Required(ErrorMessage = "Věk je povinný.")]
        [Range(18, 120, ErrorMessage = "Poradce musí být plnoletý (18+).")]
        [Display(Name = "Věk")]
        public int Vek { get; set; }
    }
}