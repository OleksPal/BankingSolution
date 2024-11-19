using api.Models;
using api.Repositories;

namespace api.Services
{
    public class BankAccountService : IBankAccountService
    {
        protected readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccount> CreateAccount(decimal balance)
        {
            if (balance < 0)
                return null;

            var newBankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = "",
                Balance = balance
            };

            await _bankAccountRepository.Insert(newBankAccount);

            return newBankAccount;
        }

        public async Task<BankAccount> Deposit(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (bankAccount is not null) 
            {
                bankAccount.Balance += amount;
                var updatedBankAccount = await _bankAccountRepository.Update(bankAccount);

                return updatedBankAccount;
            }

            return null;            
        }

        public async Task<ICollection<BankAccount>> GetAll()
        {
            var bankAccounts = await _bankAccountRepository.GetAll();

            return bankAccounts;
        }

        public async Task<BankAccount> GetBankAccountByNumber(string number)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            return bankAccount;
        }

        public async Task<BankAccount> Transfer(string senderNumber, string recipientNumber, decimal amount)
        {
            var sender = await _bankAccountRepository.GetByNumber(senderNumber);

            if (sender is not null) 
            {
                var recipient = await _bankAccountRepository.GetByNumber(recipientNumber);

                if (recipient is not null)
                {
                    await Withdraw(senderNumber, amount);
                    var updatedRecipient = await Deposit(recipientNumber, amount);

                    return updatedRecipient;
                }
            }

            return null;
        }

        public async Task<BankAccount> Withdraw(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (bankAccount is not null && bankAccount.Balance >= amount)
            {
                bankAccount.Balance -= amount;
                var updatedBankAccount = await _bankAccountRepository.Update(bankAccount);

                return updatedBankAccount;
            }

            return null;
        }
    }
}
