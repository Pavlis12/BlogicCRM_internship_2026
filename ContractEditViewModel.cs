namespace BlogicCRM.ViewModels
{
    public class ContractEditViewModel : ContractFormViewModel
    {
        public int Id { get; set; }
        public DateTime DatumUzavreni { get; set; } = DateTime.Today;
        public DateTime DatumPlatnosti { get; set; } = DateTime.Today;
        public DateTime? DatumUkonceni { get; set; }
    }
}