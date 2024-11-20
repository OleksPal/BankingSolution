using api.Models;

namespace api.Repositories
{
    public interface IBankAccountRepository
    {
        Task<ICollection<BankAccount>> GetAll();
        Task<BankAccount> GetByNumber(string accountNumber);
        Task<BankAccount> Insert(BankAccount bankAccount);
        Task<BankAccount> Update(string accountNumber, decimal newBalance);
    }
}
