using System.ComponentModel.DataAnnotations;

namespace CurrencyWallet.DTO.Models
{
    public class Transfer
    {
        private string _currency;
        public string Currency {
            get { return _currency; }
            set { _currency = value.ToUpper(); }
        }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
