using api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tests.Helpers;

namespace tests.Repository_tests
{
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
    }
}
