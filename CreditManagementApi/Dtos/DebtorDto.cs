using System.ComponentModel.DataAnnotations;

namespace CreditManagementApi.Dtos
{
    public class DebtorDto
    {
        public Guid Id { get; set; }
        public string Fincode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime CreditStartDate { get; set; } = DateTime.Now;
        public int CreditTermMonths { get; set; }
    }
}
