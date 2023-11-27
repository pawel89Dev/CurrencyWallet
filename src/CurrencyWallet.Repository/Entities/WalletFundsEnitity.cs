using CurrencyWallet.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Repository.Entities
{
    public class WalletFundsEnitity
    {
        public int RateId { get; set; }
        public RateEntity Rate { get; set; }

        public int WalletId { get; set; }
        public WalletEntity Wallet { get; set; }

        public decimal Amount { get; set; }
    }
}
