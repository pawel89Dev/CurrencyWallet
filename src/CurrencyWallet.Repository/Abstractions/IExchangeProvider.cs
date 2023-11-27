using CurrencyWallet.DTO.Models;

namespace CurrencyWallet.Repository.Abstractions
{
    public interface IExchangeProvider
    {
        public Task<Transfer> ExchangeCurrency(Exchange exchange); 
    }
}
