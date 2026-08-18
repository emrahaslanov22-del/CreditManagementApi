using System.ComponentModel.DataAnnotations;

namespace CreditManagementApi.Dtos
{
    public class CreateDebtorDto
    {
        [Required]
        [StringLength(7, MinimumLength = 7)]
        public string Fincode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CreditBalance { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CurrentBalance { get; set; }

        [Required]
        public DateTime CreditStartDate { get; set; } = DateTime.Now;

        [Required]
        [Range(1, 360)]
        public int CreditTermMonths { get; set; }
    }
}
