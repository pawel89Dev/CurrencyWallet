using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;

namespace CurrencyWallet.Core.Abstractions
{
    public interface IWalletService
    {
        public Task<List<Wallet>> GellAllWallets();
        public Task<bool> CreateWallet(Wallet wallet);
        public Task SaveRates(NBPResponse rates);
        public Task DepositeFunds(int walletId, Transfer deposite);
        public Task WithdrawFunds(int walletId, Transfer deposite);
        public Task ExchangeFund(int walletId, Exchange exchange);
    }
}
