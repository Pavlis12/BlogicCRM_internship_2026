using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogicCRM.ViewModels
{
    public class ContractFormViewModel
    {
        [Required(ErrorMessage = "Evidenční číslo je povinné.")]
        [Display(Name = "Evidenční číslo smlouvy")]
        public string EvidencniCislo { get; set; }

        [Required(ErrorMessage = "Název instituce je povinný.")]
        [Display(Name = "Instituce (pojišťovna/banka)")]
        public string Instituce { get; set; }

        [Required(ErrorMessage = "Musíte vybrat klienta.")]
        [Display(Name = "Klient")]
        public int ClientId { get; set; }

        public List<SelectListItem>? DostupniKlienti { get; set; }

        [Required(ErrorMessage = "Musíte vybrat správce smlouvy.")]
        [Display(Name = "Správce smlouvy (hlavní poradce)")]
        public int ManagerId { get; set; }

        public List<SelectListItem>? DostupniManageri { get; set; }


        [Display(Name = "Další účastníci (poradci)")]
        public List<AdvisorCheckboxViewModel> DostupniUcastnici { get; set; } = new();
        public DateTime DatumUzavreni { get; set; } = DateTime.Today;
        public DateTime DatumPlatnosti { get; set; } = DateTime.Today;
        public DateTime? DatumUkonceni { get; set; }
    }
}