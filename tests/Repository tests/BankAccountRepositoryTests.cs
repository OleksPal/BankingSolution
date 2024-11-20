using api.Models;
using api.Repositories;
using tests.Helpers;

namespace tests.Repository_tests
{
    [Collection("TestCollection")]
    public class BankAccountRepositoryTests
    {
        protected readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountRepositoryTests()
        {
            _bankAccountRepository = Helper.GetRequiredService<IBankAccountRepository>()
                ?? throw new ArgumentNullException(nameof(IBankAccountRepository));
        }

        #region GetAll
        [Fact]
        public async Task GetAll_ReturnsAccountsList()
        {
            // Act
            var accountList = await _bankAccountRepository.GetAll();

            // Assert
            Assert.NotEmpty(accountList);
        }
        #endregion

        #region GetByNumber
        [Fact]
        public async Task GetByNumber_NonExistentAccount_ReturnsNull()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            var bankAccount = await _bankAccountRepository.GetByNumber(nonExistentAccountNumber);

            // Assert
            Assert.Null(bankAccount);
        }

        [Fact]
        public async Task GetByNumber_ExistingAccount_ReturnsBankAccount()
        {
            // Arrange
            var existingAccountNumber = "UAYYZZZZZZ0000012345678901234";

            // Act
            var bankAccount = await _bankAccountRepository.GetByNumber(existingAccountNumber);

            // Assert
            Assert.NotNull(bankAccount);
        }
        #endregion

        #region Insert
        [Fact]
        public async Task Insert_Null_ReturnsArgumentNullException()
        {
            // Arrange
            BankAccount bankAccount = null;

            // Act
            Func<Task> act = () => _bankAccountRepository.Insert(bankAccount);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task Insert_ValidBankAccount_ReturnsSameAccount()
        {
            // Arrange
            var bankAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountNumber = Guid.NewGuid().ToString(),
                Balance = 1000
            };

            // Act
            var addedBankAccount = await _bankAccountRepository.Insert(bankAccount);

            // Assert
            Assert.Same(bankAccount, addedBankAccount);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_Null_ReturnsNullReferenceException()
        {
            // Arrange
            string bankAccountNumber = null;

            // Act
            Func<Task> act = () => _bankAccountRepository.Update(bankAccountNumber, 100);

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(act);
        }

        [Fact]
        public async Task Update_NonExistentBankAccount_ReturnsNullReferenceException()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            Func<Task> act = () => _bankAccountRepository.Update(nonExistentAccountNumber, 1000);

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(act);
        }

        [Fact]
        public async Task Update_ValidBankAccount_ReturnsSameAccount()
        {
            // Arrange
            var existingAccountNumber = "UAYYZZZZZZ0000012345678901234";
            var newBankAccountBalance = 1000;

            // Act
            var updatedBankAccount = await _bankAccountRepository.Update(existingAccountNumber, newBankAccountBalance);

            // Assert
            Assert.Equal(newBankAccountBalance, updatedBankAccount.Balance);
        }
        #endregion
    }
}
