using api.Dtos;
using api.Mappers;
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
            var updatedBankAccount = await DepositFunds(number, amount);
            await _bankAccountRepository.SaveChanges();

            return updatedBankAccount;            
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

        public async Task<ICollection<BankAccountDto>> Transfer(string senderNumber, string recipientNumber, decimal amount)
        {
            if (senderNumber == recipientNumber) 
                throw new ArgumentOutOfRangeException("You can`t transfer funds to the same bank account");

            var sender = await WithdrawFunds(senderNumber, amount);
            var recipient = await DepositFunds(recipientNumber, amount);
            await _bankAccountRepository.SaveChanges();

            var transactionParticipants = new List<BankAccountDto>();
            transactionParticipants.Add(sender);
            transactionParticipants.Add(recipient);

            return transactionParticipants;
        }

        public async Task<BankAccountDto> Withdraw(string number, decimal amount)
        {
            var updatedBankAccount = await WithdrawFunds(number, amount);
            await _bankAccountRepository.SaveChanges();

            return updatedBankAccount;
        }

        private async Task<BankAccountDto> DepositFunds(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Amount should be positive");

            if (bankAccount is null)
                throw new ArgumentException($"Bank account with number {number} doesn`t exists");

            var newBalance = bankAccount.Balance + amount;
            var updatedBankAccount = await _bankAccountRepository.Update(number, newBalance);

            return updatedBankAccount.ToBankAccountDto();
        }

        private async Task<BankAccountDto> WithdrawFunds(string number, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByNumber(number);

            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Amount should be positive");

            if (bankAccount is null)
                throw new ArgumentException($"Bank account with number {number} doesn`t exists");

            if (bankAccount.Balance < amount)
                throw new ArgumentException($"Bank account with number {number} doesn`t have enough funds to withdraw");

            var newBalance = bankAccount.Balance - amount;
            var updatedBankAccount = await _bankAccountRepository.Update(number, newBalance);

            return updatedBankAccount.ToBankAccountDto();
        }
    }
}
