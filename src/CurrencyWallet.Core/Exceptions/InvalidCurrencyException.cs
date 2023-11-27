namespace CurrencyWallet.Core.Exceptions
{
    public class InvalidCurrencyException : BaseException
    {
        public string Currency { get; }

        public InvalidCurrencyException(string currency) : base($"Currency with code: '{currency}', done not exist.")
        {
            Currency = currency;
        }
    }
}
