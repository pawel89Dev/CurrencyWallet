using CurrencyWallet.Core.Abstractions;
using CurrencyWallet.Core.Component;
using CurrencyWallet.Core.Exceptions;
using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;
using CurrencyWallet.Repository.Abstractions;

namespace CurrencyWallet.Core.Services
{
    public class WalletService : IWalletService
    {
        private IWalletRepository walletRepository { get; set; }
        private IExchangeProvider exchangeProvider { get; set; }
        public WalletService(IWalletRepository _walletRepository, IExchangeProvider _exchangeProvider)
        {
            if (_walletRepository == null) throw new ArgumentNullException(nameof(_walletRepository));
            if (_exchangeProvider == null) throw new ArgumentNullException(nameof(_exchangeProvider));
            walletRepository = _walletRepository;
            exchangeProvider = _exchangeProvider;
        }   

        public async Task<List<Wallet>> GellAllWallets()
        {
            var wallets = await walletRepository.GetWallets();
            return wallets;
        }

        public async Task SaveRates(NBPResponse rates)
        {
            await walletRepository.SaveRates(rates);
        }

        public async Task<bool> CreateWallet(Wallet wallet)
        {
            if (await walletRepository.IsWalletNameExist(wallet))
            {
                throw new WalletExistException(wallet.Name);
            }
            return await walletRepository.CreateWallet(wallet);
        }

        public async Task DepositeFunds(int walletId, Transfer deposite)
        {
            if (await walletRepository.IsWalletNotExist(walletId))
            {
                throw new InvalidWalletException(walletId);
            }

            if (await walletRepository.IsCurrencyNotExist(deposite.Currency))
            {
                throw new InvalidCurrencyException(deposite.Currency);
            }

            await walletRepository.DepositeFunds(walletId, deposite);
        }

        public async Task WithdrawFunds(int walletId, Transfer deposite)
        {
            if (await walletRepository.IsWalletNotExist(walletId))
            {
                throw new InvalidWalletException(walletId);
            }

            if (await walletRepository.IsCurrencyNotExist(deposite.Currency))
            {
                throw new InvalidCurrencyException(deposite.Currency);
            }

            if (await walletRepository.IsNoFundsInWallet(walletId, deposite))
            {
                throw new NotEnoughFundsException(walletId, deposite.Currency);
            }

            await walletRepository.WithrawFunds(walletId, deposite);
        }

        public async Task ExchangeFund(int walletId, Exchange exchange)
        {
            if (await walletRepository.IsWalletNotExist(walletId))
            {
                throw new InvalidWalletException(walletId);
            }
            if (await walletRepository.IsCurrencyNotExist(exchange.CurrencyFrom))
            {
                throw new InvalidCurrencyException(exchange.CurrencyFrom);
            }
            if (await walletRepository.IsNoFundsInWallet(walletId, new Transfer { Amount = exchange.Amount, Currency = exchange.CurrencyFrom}))
            {
                throw new NotEnoughFundsException(walletId, exchange.CurrencyFrom);
            }
            Transfer transferFrom = new Transfer { Currency = exchange.CurrencyFrom, Amount = exchange.Amount };
            Transfer transferTo = await exchangeProvider.ExchangeCurrency(exchange);
           
            await walletRepository.ExchangeFund(walletId, transferFrom, transferTo);
            
            bool ja = true;

        }
    }
}
