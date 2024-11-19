using api.Dtos;
using api.Models;
using api.Repositories;
using api.Mappers;

namespace api.Services
{
    public class BankAccountService : IBankAccountService
    {
        protected readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccountDto> CreateAccount(decimal balance)
        {
            if (balance < 0)
                return null;

            var newBankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = Guid.NewGuid().ToString(), // Use guid to generate a random string
                Balance = balance
            };

            await _bankAccountRepository.Insert(newBankAccount);

            return newBankAccount.ToBankAccountDto();
        }

        public async Task<BankAccountDto> Deposit(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (bankAccount is not null) 
            {
                bankAccount.Balance += amount;
                var updatedBankAccount = await _bankAccountRepository.Update(bankAccount);

                return updatedBankAccount.ToBankAccountDto();
            }

            return null;            
        }

        public async Task<ICollection<BankAccountDto>> GetAll()
        {
            var bankAccounts = await _bankAccountRepository.GetAll();
            var bankAccountDtos = bankAccounts.Select(bankAccount => bankAccount.ToBankAccountDto());

            return bankAccountDtos.ToList();
        }

        public async Task<BankAccountDto> GetBankAccountByNumber(string number)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            return bankAccount.ToBankAccountDto();
        }

        public async Task<BankAccountDto> Transfer(string senderNumber, string recipientNumber, decimal amount)
        {
            var sender = await _bankAccountRepository.GetByNumber(senderNumber);

            if (sender is not null) 
            {
                var recipient = await _bankAccountRepository.GetByNumber(recipientNumber);

                if (recipient is not null)
                {
                    var senderAccount = await Withdraw(senderNumber, amount);

                    if (senderAccount is null)
                        return null;

                    var updatedRecipient = await Deposit(recipientNumber, amount);

                    if (updatedRecipient is null)
                        return null;

                    return updatedRecipient;
                }
            }

            return null;
        }

        public async Task<BankAccountDto> Withdraw(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (bankAccount is not null && bankAccount.Balance >= amount)
            {
                bankAccount.Balance -= amount;
                var updatedBankAccount = await _bankAccountRepository.Update(bankAccount);

                return updatedBankAccount.ToBankAccountDto();
            }

            return null;
        }
    }
}
