using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IBankAccountService
    {
        Task<ICollection<BankAccountDto>> GetAll();
        Task<BankAccountDto> GetBankAccountByNumber(string number);
        Task<BankAccountDto> CreateAccount(decimal balance);
        Task<BankAccountDto> Deposit(string number, decimal amount);
        Task<BankAccountDto> Withdraw(string number, decimal amount);
        Task<ICollection<BankAccountDto>> Transfer(string senderNumber, string recipientNumber, decimal amount);
    }
}
