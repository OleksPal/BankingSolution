using api.Dtos;
using api.Mappers;
using api.Models;
using api.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                throw new ArgumentOutOfRangeException("Balance should be positive");

            var newBankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = Guid.NewGuid().ToString(), // Use guid to generate a random string
                Balance = balance
            };

            await _bankAccountRepository.Insert(newBankAccount);
            await _bankAccountRepository.SaveChanges();

            return newBankAccount.ToBankAccountDto();
        }

        public async Task<BankAccountDto> Deposit(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount should be positive");

            if (bankAccount is null)
                throw new ArgumentException($"Bank account with number {number} doesn`t exists");

            var newBalance = bankAccount.Balance + amount;
            var updatedBankAccount = await _bankAccountRepository.Update(number, newBalance);
            await _bankAccountRepository.SaveChanges();

            return updatedBankAccount.ToBankAccountDto();            
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

            if (bankAccount is null)
                throw new ArgumentException($"Bank account with number {number} doesn`t exists");

            return bankAccount.ToBankAccountDto();
        }

        public async Task<BankAccountDto> Transfer(string senderNumber, string recipientNumber, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount should be positive");

            if (senderNumber == recipientNumber) 
                throw new ArgumentOutOfRangeException("You can`t transfer funds to the same bank account");

            // Withdraw funds
            var sender = await _bankAccountRepository.GetByNumber(senderNumber);

            if (sender is null)
                throw new ArgumentException($"Bank account with number {senderNumber} doesn`t exists");

            if (sender.Balance < amount)
                throw new ArgumentException($"Bank account with number {senderNumber} doesn`t have enough funds to withdraw");

            var newSenderBalance = sender.Balance - amount;
            await _bankAccountRepository.Update(senderNumber, newSenderBalance);

            // Deposit funds
            var recipient = await _bankAccountRepository.GetByNumber(recipientNumber);

            if (recipient is null)
                throw new ArgumentException($"Bank account with number {recipientNumber} doesn`t exists");

            var newRecipientBalance = recipient.Balance + amount;
            var updatedRecipient = await _bankAccountRepository.Update(recipientNumber, newRecipientBalance);
            await _bankAccountRepository.SaveChanges();

            return updatedRecipient.ToBankAccountDto();
        }

        public async Task<BankAccountDto> Withdraw(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount should be positive");

            if (bankAccount is null)
                throw new ArgumentException($"Bank account with number {number} doesn`t exists");

            if (bankAccount.Balance < amount)
                throw new ArgumentException($"Bank account with number {number} doesn`t have enough funds to withdraw");

            var newBalance = bankAccount.Balance - amount;
            var updatedBankAccount = await _bankAccountRepository.Update(number, newBalance);
            await _bankAccountRepository.SaveChanges();

            return updatedBankAccount.ToBankAccountDto();
        }
    }
}
