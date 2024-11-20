using api.Controllers;
using api.Dtos;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using tests.Helpers;

namespace tests.ControllerTests
{
    [Collection("TestCollection")]
    public class BankAccountControllerTests
    {
        protected readonly BankAccountController _bankAccountController;
        protected readonly string ExistingAccountNumber = "UAYYZZZZZZ0000012345678901234";

        public BankAccountControllerTests()
        {
            var bankAccountService = Helper.GetRequiredService<IBankAccountService>()
                ?? throw new ArgumentNullException(nameof(IBankAccountService));

            _bankAccountController = new BankAccountController(bankAccountService);
        }

        #region GetAll
        [Fact]
        public async Task GetAll_ReturnsAccountList()
        {
            // Act
            var actionResult = await _bankAccountController.GetAll();

            // Assert
            var okResult = actionResult as OkObjectResult;
            var accountList = okResult.Value as ICollection<BankAccountDto>;
            Assert.NotEmpty(accountList);
        }
        #endregion

        #region GetBankAccount
        [Fact]
        public async Task GetBankAccount_Null_ReturnsNotFound()
        {
            // Arrange
            string nonExistentAccountNumber = null;

            // Act
            var actionResult = await _bankAccountController.GetBankAccount(nonExistentAccountNumber);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetBankAccount_NonExistentAccount_ReturnsNotFound()
        {
            // Assert
            var nonExistentAccountNumber = String.Empty;

            // Act
            var actionResult = await _bankAccountController.GetBankAccount(nonExistentAccountNumber);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetBankAccount_ExistingAccount_ReturnsOkBankAccount()
        {
            // Act
            var actionResult = await _bankAccountController.GetBankAccount(ExistingAccountNumber);

            // Assert
            var okResult = actionResult as OkObjectResult;
            var bankAccount = okResult.Value as BankAccountDto;
            Assert.NotNull(bankAccount);
        }
        #endregion

        #region CreateBankAccount
        [Fact]
        public async Task CreateBankAccount_NegativeBalance_ReturnsBadRequest()
        {
            // Arrange
            var negativeStartBalance = -5;

            // Act
            var actionResult = await _bankAccountController.CreateBankAccount(negativeStartBalance);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task CreateBankAccount_PositiveBalance_ReturnsOkNewBankAccount()
        {
            // Arrange
            var startBalance = 100;

            // Act
            var actionResult = await _bankAccountController.CreateBankAccount(startBalance);

            // Assert
            var okResult = actionResult as OkObjectResult;
            var bankAccount = okResult.Value as BankAccountDto;
            Assert.NotNull(bankAccount);
        }
        #endregion

        #region Deposit
        [Fact]
        public async Task Deposit_NonExistentAccount_ReturnsNotFound()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            var actionResult = await _bankAccountController.Deposit(nonExistentAccountNumber, 100);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task Deposit_NegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            var negativeDepositAmount = -5;

            // Act
            var actionResult = await _bankAccountController.Deposit(ExistingAccountNumber, negativeDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Deposit_ZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            var zeroDepositAmount = 0;

            // Act
            var actionResult = await _bankAccountController.Deposit(ExistingAccountNumber, zeroDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Deposit_ExistingUser_PositiveAmount_ReturnsOkUpdatedAccount()
        {
            // Arrange
            var depositAmount = 100;
            var expectedBalance = 200;

            // Act
            var actionResult = await _bankAccountController.Deposit(ExistingAccountNumber, depositAmount);

            // Assert
            var okResult = actionResult as OkObjectResult;
            var bankAccount = okResult.Value as BankAccountDto;
            Assert.Equal(expectedBalance, bankAccount.Balance);
        }
        #endregion

        #region Withdraw
        [Fact]
        public async Task Withdraw_NonExistentAccount_ReturnsBadRequest()
        {
            // Arrange
            var nonExistentAccountNumber = String.Empty;

            // Act
            var actionResult = await _bankAccountController.Withdraw(nonExistentAccountNumber, 100);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Withdraw_NegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            var negativeDepositAmount = -5;

            // Act
            var actionResult = await _bankAccountController.Withdraw(ExistingAccountNumber, negativeDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Withdraw_ZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            var zeroDepositAmount = 0;

            // Act
            var actionResult = await _bankAccountController.Withdraw(ExistingAccountNumber, zeroDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Withdraw_ExistingUser_PositiveAmount_ReturnsUpdatedAccount()
        {
            // Arrange
            var depositAmount = 100;
            var expectedBalance = 0;

            // Act
            var actionResult = await _bankAccountController.Withdraw(ExistingAccountNumber, depositAmount);

            // Assert
            var okResult = actionResult as OkObjectResult;
            var bankAccount = okResult.Value as BankAccountDto;
            Assert.Equal(expectedBalance, bankAccount.Balance);
        }
        #endregion

        #region Transfer
        [Fact]
        public async Task Transfer_SameAccount_ReturnsNotFound()
        {
            // Act
            var actionResult = await _bankAccountController.Transfer(ExistingAccountNumber, ExistingAccountNumber, 100);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task Transfer_NegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var addActionResult = await _bankAccountController.CreateBankAccount(100);
            var recipient = (addActionResult as OkObjectResult).Value as BankAccountDto;
            var negativeDepositAmount = -5;

            // Act
            var actionResult = await _bankAccountController
            .Transfer(senderAccountNumber, recipient.AccountNumber, negativeDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Transfer_ZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var addActionResult = await _bankAccountController.CreateBankAccount(100);
            var recipient = (addActionResult as OkObjectResult).Value as BankAccountDto;
            var zeroDepositAmount = 0;

            // Act
            var actionResult = await _bankAccountController
            .Transfer(senderAccountNumber, recipient.AccountNumber, zeroDepositAmount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Transfer_ExistingUser_PositiveAmount_ReturnsUpdatedAccounts()
        {
            // Arrange
            var senderAccountNumber = ExistingAccountNumber;
            var addActionResult = await _bankAccountController.CreateBankAccount(100);
            var recipient = (addActionResult as OkObjectResult).Value as BankAccountDto;
            var amount = 100;
            var senderExpectedBalance = 0;
            var recipientExpectedBalance = 200;

            // Act
            var actionResult = await _bankAccountController
                .Transfer(senderAccountNumber, recipient.AccountNumber, amount);

            // Assert
            var okResult = actionResult as OkObjectResult;
            var accountList = okResult.Value as ICollection<BankAccountDto>;

            var senderBalance = accountList.First(account => account.AccountNumber == senderAccountNumber).Balance;
            var recipientBalance = accountList.First(account => account.AccountNumber == recipient.AccountNumber).Balance;

            Assert.Multiple(
                () => Assert.Equal(senderExpectedBalance, senderBalance),
                () => Assert.Equal(recipientExpectedBalance, recipientBalance));
        }
        #endregion
    }
}
