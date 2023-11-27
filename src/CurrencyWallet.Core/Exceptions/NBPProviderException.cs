using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Core.Exceptions
{
    public class NBPProviderException : BaseException
    {
        public NBPProviderException(Exception ex) : base($"Unable to download rates from NBP website", ex)
        {
        }
    }
}
