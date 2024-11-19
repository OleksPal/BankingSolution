using api.Models;

namespace api.Services
{
    public interface IBankAccountService
    {
        Task<ICollection<BankAccount>> GetAll();
        Task<BankAccount> GetBankAccountByNumber(string number);
        Task<BankAccount> CreateAccount(decimal balance);
        Task<BankAccount> Deposit(string number, decimal amount);
        Task<BankAccount> Withdraw(string number, decimal amount);
        Task<BankAccount> Transfer(string senderNumber, string recipientNumber, decimal amount);
    }
}
