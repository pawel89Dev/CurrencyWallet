using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.DTO.Models
{
    public class NBPResponse
    {
        public string Table{ get; set; }
        public string No { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public List<Rate> Rates { get; set; }
    }

    public class Rate
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
    }
}
