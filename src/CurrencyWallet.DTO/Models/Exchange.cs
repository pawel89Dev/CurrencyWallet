using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWallet.DTO.Models
{
    public class Exchange
    {
        private string _currencyFrom;
        public string CurrencyFrom
        {
            get { return _currencyFrom; }
            set { _currencyFrom = value.ToUpper(); }
        }
        private string _currencyTo;
        public string CurrencyTo
        {
            get { return _currencyTo; }
            set { _currencyTo = value.ToUpper(); }
        }
        [Range(0.01, double.MaxValue, ErrorMessage = "The value must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
