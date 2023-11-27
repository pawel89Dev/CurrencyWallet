using CurrencyWallet.Repository.Abstractions;
using CurrencyWallet.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyWallet.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCurrencyWalletsRepository(this IServiceCollection services)
        {
            services.AddScoped<IWalletRepository, WalletRepository>();

            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<CurrencyWalletDbContext>(o =>
            {
                o.UseNpgsql(configuration.GetSection("Database:ConnectionStrings").Value);
            });

            return services;
        }
    }
}
