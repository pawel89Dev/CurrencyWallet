namespace CurrencyWallet.Repository.Entities
{
    public class WalletEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<WalletFundsEnitity> WalletFunds { get; set; }
    }
}
