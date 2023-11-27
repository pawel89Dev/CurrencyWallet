using CurrencyWallet.DTO;
using CurrencyWallet.Repository.Entities;

namespace CurrencyWallet.Repository.Mapper
{
    public static class MapperService
    {
        public static Wallet MapWalletEntityToDTO(this WalletEntity wallet)
        {
            return new Wallet
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Funds = wallet.WalletFunds.Select(wf => new DTO.Models.Transfer
                {
                    Amount = wf.Amount,
                    Currency = wf.Rate.Code
                }).ToList()
            };
        }
    }
}
