using api.Models;
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
    }
}
