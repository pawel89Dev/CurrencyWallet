using CurrencyWallet.DTO.Models;

namespace CurrencyWallet.Core.Abstractions
{
    public interface ICurrencyRatesProvider
    {
        public Task<NBPResponse> GetRates();
    }
}
