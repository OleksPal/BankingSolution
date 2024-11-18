using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class BankAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        [Range(0.0, double.PositiveInfinity)]
        public decimal Balance { get; set; }
    }
}
