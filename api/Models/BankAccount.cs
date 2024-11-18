using Microsoft.EntityFrameworkCore;
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
        [Precision(19, 4)]
        [Range(0, Double.MaxValue)]
        public decimal Balance { get; set; }
    }
}
