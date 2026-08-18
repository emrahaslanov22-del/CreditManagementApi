using System.ComponentModel.DataAnnotations;

namespace CreditManagementApi.Dtos
{
    public class UpdateDebtorDto
    {
        public Guid Id { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CreditBalance { get; set; }
    }
}
