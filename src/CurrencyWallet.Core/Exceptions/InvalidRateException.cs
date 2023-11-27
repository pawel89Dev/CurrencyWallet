using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Core.Exceptions
{
    public class InvalidRateException : BaseException
    {
        public string Currency { get; }

        public InvalidRateException(string currency) : base($"There is no exchange rate for currency: '{currency}'.")
        {
            Currency = currency;
        }
    }
}
