using CurrencyWallet.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyWallet.Core.Services
{
    public class RatesService : BackgroundService, IDisposable
    {
        private readonly int _ratesUpdateInterval = 10;
        private readonly ILogger<RatesService> _logger;
        private readonly ICurrencyRatesProvider _currencyRatesProvider;
        private Timer? _timer = null;
        public IServiceProvider Services { get; }


        public RatesService(ILogger<RatesService> logger, ICurrencyRatesProvider currencyRatesProvider, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
            _currencyRatesProvider = currencyRatesProvider;

        }

        private async Task GetRates()
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();
                    var data = await _currencyRatesProvider.GetRates();
                    if (data != null)
                    {
                        await walletService.SaveRates(data);
                    }
                    _logger.LogInformation("NBP rates was stored on DB");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occured in {nameof(GetRates)}:");
                }
            }
           
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(RatesService)} running.");
            _timer = new Timer(async (_) => await GetRates(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_ratesUpdateInterval));

            return Task.CompletedTask;
        }
    }
}
