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
    }
}
