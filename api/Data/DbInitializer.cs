using api.Models;

namespace api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BankingSolutionContext context)
        {
            if (context.BankAccounts.Any())
                return;

            var bankAccout = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = "UAYYZZZZZZ0000012345678901234",
                Balance = 100.0m
            };

            context.BankAccounts.Add(bankAccout);

            context.SaveChanges();
        }
    }
}
