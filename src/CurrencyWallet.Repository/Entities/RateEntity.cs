using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.Repository.Entities
{
    public class RateEntity
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
