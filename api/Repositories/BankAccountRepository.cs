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

            return bankAccount;
        }        

        public async Task<BankAccount> Update(string accountNumber, decimal newBalance)
        {
            var bankAccount = await GetByNumber(accountNumber);
            bankAccount.Balance = newBalance;            

            return bankAccount;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
