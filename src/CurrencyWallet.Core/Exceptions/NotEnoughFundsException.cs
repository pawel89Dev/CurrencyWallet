using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Core.Exceptions
{
    public class NotEnoughFundsException : BaseException
    {
        public int WalletId { get; set; }
        public string Currency { get; }

        public NotEnoughFundsException(int walletId, string currency) : base($"Not enough funds for currency: '{currency}', in wallet: {walletId}.")
        {
            WalletId = walletId;
            Currency = currency;
        }
    }
}
