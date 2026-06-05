namespace BlogicCRM.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string RodneCislo { get; set; }
        public int Vek { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
