using CurrencyWallet.Core.Exceptions;
using CurrencyWallet.DTO.Models;
using CurrencyWallet.Repository.Abstractions;

namespace CurrencyWallet.Core.Component
{
    public class ExchangeProvider : IExchangeProvider
    {
        private IWalletRepository _walletRepository { get; set; }
        public ExchangeProvider(IWalletRepository walletRepository)
        {
            if (walletRepository == null) throw new ArgumentNullException(nameof(walletRepository));
            _walletRepository = walletRepository;
        }
        public async Task<Transfer> ExchangeCurrency(Exchange exchange)
        {
            decimal? rateCurrencyFrom = await _walletRepository.GetCurrencyRate(exchange.CurrencyFrom);
            decimal? rateCurrencyTo = await _walletRepository.GetCurrencyRate(exchange.CurrencyTo);
            if (rateCurrencyFrom == null || rateCurrencyTo == null)
            {
                throw new InvalidRateException(rateCurrencyFrom == null ? exchange.CurrencyFrom : exchange.CurrencyTo);
            }
            decimal amountInPLN = exchange.Amount * rateCurrencyFrom.Value;
            decimal amountCurrencyTo = MarginCalculation(amountInPLN / rateCurrencyTo.Value);
            return new Transfer() {Currency = exchange.CurrencyTo,  Amount = amountCurrencyTo };

        }

        private decimal MarginCalculation(decimal originalAmount)
        {
            decimal roundedValue = Decimal.Round(originalAmount, 2, MidpointRounding.ToZero);
            //Add margin strategy to exchange in the future
            var margin = originalAmount - roundedValue;

            return roundedValue;

        }
    }
}
