using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Core.Exceptions
{
    public class InvalidWalletException : BaseException
    {
        public int WalletId { get; }

        public InvalidWalletException(int id) : base($"Wallet with id: '{id}', does not exist.")
        {
            WalletId = id;
        }
    }
}
