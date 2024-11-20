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
    }
}
