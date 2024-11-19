using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        protected readonly BankingSolutionContext _context;

        public BankAccountRepository(BankingSolutionContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BankAccount>> GetAll()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount> GetByNumber(string accountNumber)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(account => 
            account.AccountNumber == accountNumber);
        }

        public async Task<BankAccount> Insert(BankAccount bankAccount)
        {
            await _context.BankAccounts.AddAsync(bankAccount);
            await _context.SaveChangesAsync();

            return bankAccount;
        }

        public async Task<BankAccount> Update(BankAccount bankAccount)
        {
            _context.BankAccounts.Update(bankAccount);

            await _context.SaveChangesAsync();
            return bankAccount;
        }
    }
}
