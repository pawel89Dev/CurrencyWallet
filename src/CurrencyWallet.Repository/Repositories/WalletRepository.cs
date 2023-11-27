using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;
using CurrencyWallet.Repository.Abstractions;
using CurrencyWallet.Repository.Entities;
using CurrencyWallet.Repository.Mapper;
using Microsoft.EntityFrameworkCore;

namespace CurrencyWallet.Repository.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private CurrencyWalletDbContext currencyWalletDbContext;
        public WalletRepository(CurrencyWalletDbContext _currencyWalletDbContext)
        {
            if (_currencyWalletDbContext == null) throw new ArgumentNullException(nameof(_currencyWalletDbContext));
            currencyWalletDbContext = _currencyWalletDbContext;
        }

        public async Task<bool> CreateWallet(Wallet wallet)
        {
            currencyWalletDbContext.Wallets.Add(new WalletEntity
            {
                Name = wallet.Name
            });
            return await currencyWalletDbContext.SaveChangesAsync() != 0;
        }

        public async Task<List<Wallet>> GetWallets()
        {
            var currencyWallet = new List<Wallet>();
            foreach (var wallet in await currencyWalletDbContext.Wallets.
                Include(wallet => wallet.WalletFunds).
                ThenInclude(rate => rate.Rate).
                ToListAsync())
            {
                currencyWallet.Add(wallet.MapWalletEntityToDTO());
            } 
            return currencyWallet;
        }

        public async Task<bool> IsWalletNameExist(Wallet wallet)
        {
            return await currencyWalletDbContext.Wallets.AnyAsync(currWallet => currWallet.Name == wallet.Name);
        }

        public async Task<bool> IsWalletNotExist(int id)
        {
            return !await currencyWalletDbContext.Wallets.AnyAsync(currWallet => currWallet.Id == id);
        }

        public async Task<bool> IsCurrencyNotExist(string currencyCode)
        {
            return !await currencyWalletDbContext.Rates.AnyAsync(rate => rate.Code == currencyCode.ToUpper());
        }

        public async Task<bool> IsNoFundsInWallet(int walletId, Transfer deposite)
        {
            var rate = await currencyWalletDbContext.Rates.FirstOrDefaultAsync(rate => rate.Code == deposite.Currency);
            var depositeInCurrency = await currencyWalletDbContext.WalletFunds.FirstOrDefaultAsync(wf => wf.WalletId == walletId && wf.Rate == rate);
            if (depositeInCurrency != null && depositeInCurrency.Amount >= deposite.Amount)
            {
                return false;
            }

            return true;
        }

        public async Task<decimal?> GetCurrencyRate(string currencyCode)
        {
            var currencyRate = await currencyWalletDbContext.Rates.FirstOrDefaultAsync(rate => rate.Code == currencyCode.ToUpper());
            return currencyRate?.Rate;
        }

        public async Task SaveRates(NBPResponse nbpResponse)
        {
            var ratesOnDB = currencyWalletDbContext.Rates.ToList();
            foreach (var rate in nbpResponse.Rates)
            {
                if (ratesOnDB.FirstOrDefault(r => r.Code == rate.Code) is RateEntity rateOnDB)
                {
                    rateOnDB.Rate = rate.Mid;
                }
                else
                {
                    currencyWalletDbContext.Rates.Add(new Entities.RateEntity
                    {
                        Code = rate.Code,
                        Rate = rate.Mid,
                        Currency = rate.Currency
                    });
                }
            }
            await currencyWalletDbContext.SaveChangesAsync();
        }

        public async Task DepositeFunds(int walletId, Transfer deposite)
        {
            var rate = await currencyWalletDbContext.Rates.FirstOrDefaultAsync(rate => rate.Code == deposite.Currency);
            var depositeInCurrency = await currencyWalletDbContext.WalletFunds.FirstOrDefaultAsync(wf => wf.WalletId == walletId && wf.RateId == rate.Id);
            if (depositeInCurrency != null)
            {
                depositeInCurrency.Amount += deposite.Amount;
            }
            if (rate != null && depositeInCurrency == null)
            {
                currencyWalletDbContext.WalletFunds.Add(new WalletFundsEnitity
                {
                    WalletId = walletId,
                    RateId = rate.Id,
                    Amount = deposite.Amount
                });
            };
            await currencyWalletDbContext.SaveChangesAsync();
        }

        public async Task WithrawFunds(int walletId, Transfer deposite)
        {
            var rate = await currencyWalletDbContext.Rates.FirstOrDefaultAsync(rate => rate.Code == deposite.Currency);
            var depositeInCurrency = await currencyWalletDbContext.WalletFunds.FirstOrDefaultAsync(wf => wf.WalletId == walletId && wf.RateId == rate.Id);

            if (depositeInCurrency != null && depositeInCurrency.Amount >= deposite.Amount)
            {
                depositeInCurrency.Amount -= deposite.Amount;
            }
            if (depositeInCurrency != null && depositeInCurrency.Amount == 0)
            {
                currencyWalletDbContext.WalletFunds.Remove(depositeInCurrency);
            }
            await currencyWalletDbContext.SaveChangesAsync();
        }

        public async Task ExchangeFund(int walletId,Transfer transferFrom, Transfer transferTo)
        {
            using var transaction = await currencyWalletDbContext.Database.BeginTransactionAsync();

            try
            {
                await WithrawFunds(walletId, transferFrom);
                await currencyWalletDbContext.SaveChangesAsync();
                await DepositeFunds(walletId, transferTo);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
