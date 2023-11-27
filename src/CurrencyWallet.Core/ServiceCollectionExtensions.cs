using Microsoft.Extensions.DependencyInjection;
using CurrencyWallet.Repository;
using CurrencyWallet.Repository.Abstractions;
using CurrencyWallet.Core.Abstractions;
using CurrencyWallet.Core.Services;
using CurrencyWallet.Core.Component;

namespace CurrencyWallet.Core
{
    public static class ServiceCollectionExtensions
    {
    public static IServiceCollection AddCurrencyWalletsModule(this IServiceCollection services)
    {
            services.AddHostedService<RatesService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IExchangeProvider, ExchangeProvider>();
            services.AddTransient<ICurrencyRatesProvider, NBPRatesProvider>();
            services.AddCurrencyWalletsRepository();

        return services;
    }
    }
}