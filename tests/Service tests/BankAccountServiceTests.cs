using api.Models;
using api.Services;
using tests.Helpers;

namespace tests.Service_tests
{
    public class BankAccountServiceTests
    {
        protected readonly IBankAccountService _bankAccountService;

        public BankAccountServiceTests()
        {
            _bankAccountService = Helper.GetRequiredService<IBankAccountService>()
                ?? throw new ArgumentNullException(nameof(IBankAccountService));
        }

        #region GetAll
        [Fact]
        public async Task GetAll_ReturnsAccountsList()
        {
            // Act
            var accountList = await _bankAccountService.GetAll();

            // Assert
            Assert.NotEmpty(accountList);
        }
        #endregion

        #region GetBankAccountByNumber
        [Fact]
        public async Task GetBankAccountByNumber_Null_ReturnsArgumentException()
        {
            // Arrange
            string nonExistentAccountNumber = null;

            // Act
            Func<Task> act = () => _bankAccountService.GetBankAccountByNumber(nonExistentAccountNumber);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task GetBankAccountByNumber_NonExistentAccount_ReturnsArgumentException()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            Func<Task> act = () => _bankAccountService.GetBankAccountByNumber(nonExistentAccountNumber);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task GetBankAccountByNumber_ExistingAccount_ReturnsBankAccount()
        {
            // Arrange
            var existingAccountNumber = "UAYYZZZZZZ0000012345678901234";

            // Act
            var bankAccount = await _bankAccountService.GetBankAccountByNumber(existingAccountNumber);

            // Assert
            Assert.NotNull(bankAccount);
        }
        #endregion

        #region CreateAccount
        [Fact]
        public async Task CreateAccount_NegativeBalance_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var negativeStartBalance = -5;

            // Act
            Func<Task> act = () => _bankAccountService.CreateAccount(negativeStartBalance);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task CreateAccount_ZeroBalance_ReturnsNewBankAccount()
        {
            // Arrange
            var startBalance = 0;

            // Act
            var newBankAccount = await _bankAccountService.CreateAccount(startBalance);

            // Assert
            Assert.NotNull(newBankAccount);
        }

        [Fact]
        public async Task CreateAccount_PositiveBalance_ReturnsNewBankAccount()
        {
            // Arrange
            var startBalance = 100;

            // Act
            var newBankAccount = await _bankAccountService.CreateAccount(startBalance);

            // Assert
            Assert.NotNull(newBankAccount);
        }
        #endregion
    }
}
