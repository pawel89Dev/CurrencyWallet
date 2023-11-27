using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Core.Exceptions
{
    public class WalletExistException : BaseException
    {
        public string WalletName { get; }

        public WalletExistException(string name) : base($"Wallet with name: '{name}', already exist.")
        {
            WalletName = name;
        }
    }
}
