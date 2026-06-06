namespace BlogicCRM.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string EvidencniCislo { get; set; }
        public string Instituce { get; set; }
        public DateTime DatumUzavreni { get; set; }
        public DateTime DatumPlatnosti { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ManagerId { get; set; }
        public Advisor Manager { get; set; }
        public DateTime DatumZacatku { get; set; }
        public DateTime? DatumKonce { get; set; }
        public ICollection<ContractAdvisor> ContractAdvisors { get; set; }
    }
}
