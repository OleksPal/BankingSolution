using api.Services;
using tests.Helpers;

namespace tests.Service_tests
{
    [Collection("TestCollection")]
    public class BankAccountServiceTests
    {
        protected readonly IBankAccountService _bankAccountService;
        protected readonly string ExistingAccountNumber = "UAYYZZZZZZ0000012345678901234";

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
            // Act
            var bankAccount = await _bankAccountService.GetBankAccountByNumber(ExistingAccountNumber);

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

        #region Deposit
        [Fact]
        public async Task Deposit_NonExistentAccount_ReturnsArgumentException()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            Func<Task> act = () => _bankAccountService.Deposit(nonExistentAccountNumber, 100);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task Deposit_NegativeAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var negativeDepositAmount = -5;

            // Act
            Func<Task> act = () => _bankAccountService.Deposit(ExistingAccountNumber, negativeDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Deposit_ZeroAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var zeroDepositAmount = 0;

            // Act
            Func<Task> act = () => _bankAccountService.Deposit(ExistingAccountNumber, zeroDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Deposit_ExistingUser_PositiveAmount_ReturnsUpdatedAccount()
        {
            // Arrange
            var depositAmount = 100;
            var expectedBalance = 200;

            // Act
            var bankAccount = await _bankAccountService.Deposit(ExistingAccountNumber, depositAmount);

            // Assert
            Assert.Equal(expectedBalance, bankAccount.Balance);
        }
        #endregion

        #region Withdraw
        [Fact]
        public async Task Withdraw_NonExistentAccount_ReturnsArgumentException()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            Func<Task> act = () => _bankAccountService.Withdraw(nonExistentAccountNumber, 100);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task Withdraw_NegativeAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var negativeDepositAmount = -5;

            // Act
            Func<Task> act = () => _bankAccountService.Withdraw(ExistingAccountNumber, negativeDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Withdraw_ZeroAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var zeroDepositAmount = 0;

            // Act
            Func<Task> act = () => _bankAccountService.Withdraw(ExistingAccountNumber, zeroDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Withdraw_ExistingUser_PositiveAmount_ReturnsUpdatedAccount()
        {
            // Arrange
            var depositAmount = 100;
            var expectedBalance = 0;

            // Act
            var bankAccount = await _bankAccountService.Withdraw(ExistingAccountNumber, depositAmount);

            // Assert
            Assert.Equal(expectedBalance, bankAccount.Balance);
        }
        #endregion

        #region Transfer
        [Fact]
        public async Task Transfer_SameAccount_ReturnsArgumentException()
        {
            // Act
            Func<Task> act = () => _bankAccountService.Transfer(ExistingAccountNumber, ExistingAccountNumber, 100);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task Transfer_NegativeAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var recipient = await _bankAccountService.CreateAccount(100);
            var negativeDepositAmount = -5;

            // Act
            Func<Task> act = () => _bankAccountService
            .Transfer(senderAccountNumber, recipient.AccountNumber, negativeDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Transfer_ZeroAmount_ReturnsArgumentOutOfRangeException()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var recipient = await _bankAccountService.CreateAccount(100);
            var zeroDepositAmount = 0;

            // Act
            Func<Task> act = () => _bankAccountService
            .Transfer(senderAccountNumber, recipient.AccountNumber, zeroDepositAmount);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public async Task Transfer_ExistingUser_PositiveAmount_ReturnsUpdatedAccount()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var recipient = await _bankAccountService.CreateAccount(100);
            var amount = 100;
            var senderExpectedBalance = 0;
            var recipientExpectedBalance = 200;

            // Act
            var bankAccounts = await _bankAccountService
                .Transfer(senderAccountNumber, recipient.AccountNumber, amount);

            // Assert
            var senderBalance = bankAccounts.First(account => account.AccountNumber == senderAccountNumber).Balance;
            var recipientBalance = bankAccounts.First(account => account.AccountNumber == recipient.AccountNumber).Balance;

            Assert.Multiple(
                () => Assert.Equal(senderExpectedBalance, senderBalance),
                () => Assert.Equal(recipientExpectedBalance, recipientBalance));
        }
        #endregion
    }
}
