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
    }
}
