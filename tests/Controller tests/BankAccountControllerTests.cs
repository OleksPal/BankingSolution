using api.Controllers;
using api.Dtos;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using tests.Helpers;

namespace tests.Controller_tests
{
    public class BankAccountControllerTests
    {
        protected readonly BankAccountController _bankAccountController;

        public BankAccountControllerTests()
        {
            var bankAccountService = Helper.GetRequiredService<IBankAccountService>()
                ?? throw new ArgumentNullException(nameof(IBankAccountService));

            _bankAccountController = new BankAccountController(bankAccountService);
        }

        #region GetAll
        [Fact]
        public async Task GetAll_ReturnsAccountsList()
        {
            // Act
            var actionResult = await _bankAccountController.GetAll();

            // Assert
            var okResult = actionResult as OkObjectResult;
            var accountList = okResult.Value as ICollection<BankAccountDto>;
            Assert.NotEmpty(accountList);
        }
        #endregion
    }
}
