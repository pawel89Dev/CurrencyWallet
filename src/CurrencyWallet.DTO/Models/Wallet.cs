using CurrencyWallet.DTO.Models;

namespace CurrencyWallet.DTO
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transfer> Funds {get; set;}
    }
}
