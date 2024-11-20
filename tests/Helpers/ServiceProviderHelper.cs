using api.Data;
using api.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace tests.Helpers
{
    public static class Helper
    {
        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IBankAccountService, BankAccountService>();

            services.AddDbContext<BankingSolutionContext>(o => o.UseInMemoryDatabase("TestDB"));

            return services.BuildServiceProvider();
        }

        public static T GetRequiredService<T>()
        {
            var provider = Provider();

            var requiredService = provider.GetRequiredService<T>();

            var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BankingSolutionContext>();

            AddData(context);

            return requiredService;
        }

        private static void AddData(BankingSolutionContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
    }
}
