using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class ClientFormViewModel
    {
        [Required(ErrorMessage = "Jméno je povinné.")]
        [Display(Name = "Křestní jméno")]
        public string Jmeno { get; set; }

        [Required(ErrorMessage = "Příjmení je povinné.")]
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; }

        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
        [Display(Name = "E-mailová adresa")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon je povinný.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Rodné číslo je povinné.")]
        [Display(Name = "Rodné číslo")]
        public string RodneCislo { get; set; }

        [Required(ErrorMessage = "Věk je povinný.")]
        [Range(18, 120, ErrorMessage = "Klient musí být starší 18 let.")]
        public int Vek { get; set; }
    }
}