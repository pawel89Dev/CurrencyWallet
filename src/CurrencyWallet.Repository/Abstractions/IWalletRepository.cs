using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;

namespace CurrencyWallet.Repository.Abstractions
{
    public interface IWalletRepository
    {
        public Task<List<Wallet>> GetWallets();
        public Task SaveRates(NBPResponse nbpResponse);
        public Task<bool> CreateWallet(Wallet wallet);
        public Task<bool> IsWalletNameExist(Wallet wallet);
        public Task<bool> IsWalletNotExist(int id);
        public Task<bool> IsCurrencyNotExist(string currencyCode);
        public Task<bool> IsNoFundsInWallet(int walletId, Transfer deposite);
        public Task<decimal?> GetCurrencyRate(string currencyCode);
        public Task DepositeFunds(int walletId, Transfer deposite);
        public Task WithrawFunds(int walletId, Transfer deposite);
        public Task ExchangeFund(int walletId, Transfer transferFrom, Transfer transferTo);
    }
}
