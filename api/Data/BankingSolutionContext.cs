using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class BankingSolutionContext : DbContext
    {
        public BankingSolutionContext(DbContextOptions<BankingSolutionContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
