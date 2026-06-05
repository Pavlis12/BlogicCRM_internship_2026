namespace BlogicCRM.Models
{
    public class ContractAdvisor
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int AdvisorId { get; set; }
        public Advisor Advisor { get; set; }
    }
}
