using CurrencyWallet.DTO;
using CurrencyWallet.DTO.Models;
using CurrencyWallet.Repository.Abstractions;
using CurrencyWallet.Repository.Entities;
using CurrencyWallet.Repository.Repositories;
using CurrencyWallet.Repository.UnitTest;
using NUnit.Framework;

namespace CurrencyWallet.Repository.UnitTest.Repositories
{
    public class WalletRepositoryTest
    {
        [SetUp]
        public void Before()
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.SaveChanges(); ;
            }
        }

        [Test]
        public async Task GetWallets_WhenDBIsEmpty_ReturnEmptyList()
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.IsEmpty(wallets);
            }
        }
        [Test]
        public async Task GetWallets_WhenWalletsExist_ReturnAllWallets()
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel1" });
                context.Wallets.Add(new WalletEntity { Id = 2, Name = "portfel2" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                var wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.Count, Is.EqualTo(2));
                Assert.IsTrue(wallets.Any(w => w.Name == "portfel1"));
                Assert.IsTrue(wallets.Any(w => w.Name == "portfel2"));
            }
        }

        [TestCase(0)]
        [TestCase(3)]
        [TestCase(30)]
        [TestCase(678)]
        public async Task IsWalletNotExist_WhenWalletNoExist_ReturnTrue(int walletId)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel1" });
                context.Wallets.Add(new WalletEntity { Id = 2, Name = "portfel2" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isWalletsExist = await walletRepositor.IsWalletNotExist(walletId);

                Assert.IsTrue(isWalletsExist);

            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task IsWalletNotExist_WhenWalletsIsEmpty_ReturnTrue(int walletId)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isWalletsExist = await walletRepositor.IsWalletNotExist(walletId);

                Assert.IsTrue(isWalletsExist);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task IsWalletNotExist_WhenWalletsExist_ReturnFalse(int walletId)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel1" });
                context.Wallets.Add(new WalletEntity { Id = 2, Name = "portfel2" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isWalletsExist = await walletRepositor.IsWalletNotExist(walletId);

                Assert.IsFalse(isWalletsExist);
            }
        }

        [TestCase("PLN")]
        [TestCase("EUR")]
        [TestCase("USD")]
        [TestCase("testowaWaluta")]
        public async Task IsCurrencyNotExist_WhenCurrencyNoExist_ReturnTrue(string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Rates.Add(new RateEntity { Id = 1, Code = "AED", Currency = "dirham ZEA (Zjednoczone Emiraty Arabskie)" });
                context.Rates.Add(new RateEntity { Id = 2, Code = "GEL", Currency = "lari (Gruzja)" });
                context.Rates.Add(new RateEntity { Id = 3, Code = "UGX", Currency = "szyling ugandyjski" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isCurrencyExist = await walletRepositor.IsCurrencyNotExist(currencyCode);

                Assert.IsTrue(isCurrencyExist);
            }
        }

        [TestCase("PLN")]
        [TestCase("EUR")]
        [TestCase("USD")]
        [TestCase("testowaWaluta")]
        public async Task IsCurrencyNotExist_WhenCurrencyIsEmpty_ReturnTrue(string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isCurrencyExist = await walletRepositor.IsCurrencyNotExist(currencyCode);

                Assert.IsTrue(isCurrencyExist);
            }
        }

        [TestCase("AED")]
        [TestCase("GEL")]
        [TestCase("UGX")]
        public async Task IsCurrencyNotExist_WhenCurrencyExist_ReturnFalse(string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Rates.Add(new RateEntity { Id = 1, Code = "AED", Currency = "dirham ZEA (Zjednoczone Emiraty Arabskie)" });
                context.Rates.Add(new RateEntity { Id = 2, Code = "GEL", Currency = "lari (Gruzja)" });
                context.Rates.Add(new RateEntity { Id = 3, Code = "UGX", Currency = "szyling ugandyjski" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isCurrencyExist = await walletRepositor.IsCurrencyNotExist(currencyCode);

                Assert.IsFalse(isCurrencyExist);
            }
        }

        [TestCase(1, 45.000001, "AED")]
        [TestCase(1, 46, "AED")]
        [TestCase(1, 50.3430001, "GEL")]
        [TestCase(1, 60.343, "GEL")]
        [TestCase(1, 45, "USD")]
        public async Task IsNoFundsInWallet_WhenFundsNoExist_ReturnTrue(int walletId, decimal amount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate1 = new RateEntity { Id = 1, Code = "AED", Currency = "dirham ZEA (Zjednoczone Emiraty Arabskie)" };
                var rate2 = new RateEntity { Id = 2, Code = "GEL", Currency = "lari (Gruzja)" };
                var wallet = new WalletEntity { Id = 1, Name = "portfel1" };
                var walletFunds1 = new WalletFundsEnitity { Wallet = wallet, Amount = 45m, Rate = rate1 };
                var walletFunds2 = new WalletFundsEnitity { Wallet = wallet, Amount = 50.343m, Rate = rate2 };
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.Wallets.Add(wallet);
                context.WalletFunds.Add(walletFunds1);
                context.WalletFunds.Add(walletFunds2);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isCurrencyExist = await walletRepositor.IsNoFundsInWallet(walletId, new Transfer { Amount = amount, Currency = currencyCode });

                Assert.IsTrue(isCurrencyExist);
            }
        }

        [TestCase(1, 45, "AED")]
        [TestCase(1, 44.99999999, "AED")]
        [TestCase(1, 50.343000, "GEL")]
        [TestCase(1, 0.343, "GEL")]
        public async Task IsNoFundsInWallet_WhenFundsExist_ReturnFalse(int walletId, decimal amount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate1 = new RateEntity { Id = 1, Code = "AED", Currency = "dirham ZEA (Zjednoczone Emiraty Arabskie)" };
                var rate2 = new RateEntity { Id = 2, Code = "GEL", Currency = "lari (Gruzja)" };
                var wallet = new WalletEntity { Id = 1, Name = "portfel1" };
                var walletFunds1 = new WalletFundsEnitity { Wallet = wallet, Amount = 45m, Rate = rate1 };
                var walletFunds2 = new WalletFundsEnitity { Wallet = wallet, Amount = 50.343m, Rate = rate2 };
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.Wallets.Add(wallet);
                context.WalletFunds.Add(walletFunds1);
                context.WalletFunds.Add(walletFunds2);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isCurrencyExist = await walletRepositor.IsNoFundsInWallet(walletId, new Transfer { Amount = amount, Currency = currencyCode });

                Assert.IsFalse(isCurrencyExist);
            }
        }

        [TestCase("portfel")]
        [TestCase("dowolna nazwa")]
        [TestCase("@#$@#$RFSSF$%$bGFhgff")]
        public async Task CreateWallet_WhenDbIsEmpty_ShouldCreateNewWallet(string walletName)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                IWalletRepository walletRepositor = new WalletRepository(context);
                await walletRepositor.CreateWallet(new Wallet { Name = walletName });
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.Count, Is.EqualTo(1));
                Assert.IsTrue(wallets.Any(w => w.Name == walletName));
            }
        }

        [TestCase("portfel")]
        [TestCase("dowolna nazwa")]
        [TestCase("@#$@#$RFSSF$%$bGFhgff")]
        public async Task CreateWallet_WhenTheWalletWithTheSameNameExist_ShouldCreateNewWallet(string walletName)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = walletName });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                await walletRepositor.CreateWallet(new Wallet { Name = walletName });
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.Count, Is.EqualTo(2));
                Assert.IsTrue(wallets.All(w => w.Name == walletName));
            }
        }

        [TestCase("portfel1")]
        [TestCase("dowolna nazwa")]
        [TestCase("@#$@#$RFSSF$%$bGFhgff")]
        public async Task IsWalletNameExist_WhenTheWalletWithTheSameNameExist_ShouldReturnTrue(string walletName)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel1" });
                context.Wallets.Add(new WalletEntity { Id = 2, Name = "dowolna nazwa" });
                context.Wallets.Add(new WalletEntity { Id = 3, Name = "@#$@#$RFSSF$%$bGFhgff" });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isWalletNameExist = await walletRepositor.IsWalletNameExist(new Wallet { Name = walletName });

                Assert.IsTrue(isWalletNameExist);
            }
        }

        [TestCase("portfel12")]
        [TestCase("dowolna nazwa3232")]
        [TestCase(" portfel1")]
        [TestCase(" dowolna nazwa ")]
        public async Task IsWalletNameExist_WhenTheWalletWithTheSameNameExist_ShouldReturnFalse(string walletName)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel1" });
                context.Wallets.Add(new WalletEntity { Id = 2, Name = "dowolna nazwa " });
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                bool isWalletNameExist = await walletRepositor.IsWalletNameExist(new Wallet { Name = walletName });

                Assert.IsFalse(isWalletNameExist);
            }
        }

        [TestCase("AED")]
        [TestCase("GYD")]
        [TestCase("IQD")]
        [TestCase(" dowolna waluta ")]
        public async Task GetCurrencyRate_WhenDbIsEmptyExist_ShouldReturnNull(string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                IWalletRepository walletRepositor = new WalletRepository(context);
                decimal? currencyRate = await walletRepositor.GetCurrencyRate(currencyCode);

                Assert.IsNull(currencyRate);
            }
        }

        [TestCase("ILS")]
        [TestCase("GYD")]
        [TestCase("IQD")]
        [TestCase(" dowolna waluta ")]
        public async Task GetCurrencyRate_WhenCurrencyNotExist_ShouldReturnNull(string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate1 = new RateEntity { Id = 1, Code = "AED", Currency = "dirham ZEA (Zjednoczone Emiraty Arabskie)" };
                var rate2 = new RateEntity { Id = 2, Code = "GEL", Currency = "lari (Gruzja)" };
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                decimal? currencyRate = await walletRepositor.GetCurrencyRate(currencyCode);

                Assert.IsNull(currencyRate);
            }
        }

        [TestCase("AED", 3.53392)]
        [TestCase("GEL", 45.79)]
        public async Task GetCurrencyRate_WhenCurrencyExist_ShouldReturnRate(string currencyCode, decimal currencyRate)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate = new RateEntity { Id = 1, Code = currencyCode, Currency = "dirham", Rate = currencyRate };
                context.Rates.Add(rate);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                decimal? currencyRateFromDB = await walletRepositor.GetCurrencyRate(currencyCode);

                Assert.IsNotNull(currencyRateFromDB);
                Assert.That(currencyRate, Is.EqualTo(currencyRateFromDB));
            }
        }

        [TestCase(1, 50, "AED")]
        [TestCase(1, 300000, "HKD")]
        public async Task DepositeFunds_WhenNoFundsInCurrencyOnWallet_ShouldStoredFundsInWallet(int walletId, decimal amount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel" });
                var rate = new RateEntity { Id = 1, Code = currencyCode, Currency = "waluta", Rate = 0.43432m };
                context.Rates.Add(rate);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets =  await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x=> x.Id == walletId)?.Funds.Count, Is.EqualTo(0));

                await walletRepositor.DepositeFunds(walletId, new Transfer { Amount = amount, Currency = currencyCode });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(amount));
            }
        }

        [TestCase(1, 50, 100, "AED")]
        [TestCase(1, 126.11, 252.22, "HKD")]
        public async Task DepositeFunds_WhenExistFundsInTheSameCurrencyOnWallet_ShouldAddedFundsInWallet(int walletId, decimal amount, decimal expectedAmount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate = new RateEntity { Id = 1, Code = currencyCode, Currency = "waluta", Rate = 32.67m };
                var wallet = new WalletEntity { Id = 1, Name = "portfel" };
                var walletFunds = new WalletFundsEnitity { Wallet = wallet, Amount = amount, Rate = rate };
                context.Wallets.Add(wallet);
                context.Rates.Add(rate);
                context.WalletFunds.Add(walletFunds);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(amount));

                await walletRepositor.DepositeFunds(walletId, new Transfer { Amount = amount, Currency = currencyCode });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(expectedAmount));
            }
        }

        [TestCase(1, 50, "AED")]
        [TestCase(1, 126.11, "HKD")]
        public async Task DepositeFunds_WhenExistFundsInTheOtherCurrencyOnWallet_ShouldAddedFundsInWallet(int walletId, decimal amount, string currencyCode)
        {
            const decimal AMOUNT_USD = 20;
            const string CURRENCY_USD = "USD";
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {

                var rate1 = new RateEntity { Id = 1, Code = CURRENCY_USD, Currency = "waluta", Rate = 5.0123m };
                var rate2 = new RateEntity { Id = 2, Code = currencyCode, Currency = "waluta", Rate = 32.67m };
                var wallet = new WalletEntity { Id = 1, Name = "portfel" };
                var walletFunds = new WalletFundsEnitity { Wallet = wallet, Amount = AMOUNT_USD, Rate = rate1 };
                context.Wallets.Add(wallet);
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.WalletFunds.Add(walletFunds);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(CURRENCY_USD));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(AMOUNT_USD));

                await walletRepositor.DepositeFunds(walletId, new Transfer { Amount = amount, Currency = currencyCode });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(2));
                Assert.IsTrue(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Any(x => x.Currency == currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds?.FirstOrDefault(x => x.Currency == currencyCode).Amount, Is.EqualTo(amount));
            }
        }

        [TestCase(1, 50, "AED")]
        [TestCase(1, 300000, "HKD")]
        public async Task WithrawFunds_WhenNoFundsInCurrencyOnWallet_ShouldDoNothing(int walletId, decimal amount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                context.Wallets.Add(new WalletEntity { Id = 1, Name = "portfel" });
                var rate = new RateEntity { Id = 1, Code = currencyCode, Currency = "waluta", Rate = 0.43432m };
                context.Rates.Add(rate);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(0));

                await walletRepositor.WithrawFunds(walletId, new Transfer { Amount = amount, Currency = currencyCode });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(0));
            }
        }

        [TestCase(1, 100, 75, 25, "AED")]
        [TestCase(1, 126.11, 34.68, 91.43, "HKD")]
        public async Task WithrawFunds_WhenExistFundsInTheSameCurrencyOnWallet_ShouldReduceFundsInWallet(int walletId, decimal totalAmount, decimal withdrawAmount, decimal expectedAmount, string currencyCode)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate = new RateEntity { Id = 1, Code = currencyCode, Currency = "waluta", Rate = 32.67m };
                var wallet = new WalletEntity { Id = 1, Name = "portfel" };
                var walletFunds = new WalletFundsEnitity { Wallet = wallet, Amount = totalAmount, Rate = rate };
                context.Wallets.Add(wallet);
                context.Rates.Add(rate);
                context.WalletFunds.Add(walletFunds);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(totalAmount));

                await walletRepositor.WithrawFunds(walletId, new Transfer { Amount = withdrawAmount, Currency = currencyCode });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(currencyCode));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(expectedAmount));
            }
        }

        [TestCase(1, "AED", 50, 24, "HGK", 45)]
        [TestCase(1, "USD", 100, 34.56, "EUR", 56.43)]
        public async Task ExchangeFund_WhenBaseCurrencyExistButQuoteDoesnt_ShouldExchangeFundsInWallet(int walletId, string baseCurrency, decimal totalAmount, decimal quoteAmount, string quoteCurrency, decimal exchangedAmount)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate1 = new RateEntity { Id = 1, Code = baseCurrency, Currency = "waluta", Rate = 32.67m };
                var rate2 = new RateEntity { Id = 2, Code = quoteCurrency, Currency = "waluta2", Rate = 32.67m };
                var wallet = new WalletEntity { Id = 1, Name = "portfel" };
                var walletFunds = new WalletFundsEnitity { Wallet = wallet, Amount = totalAmount, Rate = rate1 };
                context.Wallets.Add(wallet);
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.WalletFunds.Add(walletFunds);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(1));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Currency, Is.EqualTo(baseCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds[0].Amount, Is.EqualTo(totalAmount));

                await walletRepositor.ExchangeFund(walletId, 
                    new Transfer { Amount = quoteAmount, Currency = baseCurrency },
                    new Transfer { Amount = exchangedAmount, Currency = quoteCurrency });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(2));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Currency, Is.EqualTo(baseCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Amount, Is.EqualTo(totalAmount-quoteAmount));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Currency, Is.EqualTo(quoteCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Amount, Is.EqualTo(exchangedAmount));
            }
        }

        [TestCase(1, "AED", 50, 24, "HGK", 45, 20, 65)]
        [TestCase(1, "USD", 100, 34.56, "EUR", 56.43, 12.11, 68.54)]
        public async Task ExchangeFund_WhenBaseCurrencyAndQuoteExists_ShouldExchangeFundsInWallet(int walletId, string baseCurrency, decimal totalAmount, decimal quoteAmount, string quoteCurrency, decimal totalQuoteAmount, decimal exchangedAmount, decimal finalQuoteAmount)
        {
            using (var context = InMemoryDbContext.CreateInMemory("test"))
            {
                var rate1 = new RateEntity { Id = 1, Code = baseCurrency, Currency = "waluta", Rate = 32.67m };
                var rate2 = new RateEntity { Id = 2, Code = quoteCurrency, Currency = "waluta2", Rate = 32.67m };
                var wallet = new WalletEntity { Id = 1, Name = "portfel" };
                var walletFunds1 = new WalletFundsEnitity { Wallet = wallet, Amount = totalAmount, Rate = rate1 };
                var walletFunds2 = new WalletFundsEnitity { Wallet = wallet, Amount = totalQuoteAmount, Rate = rate2 };
                context.Wallets.Add(wallet);
                context.Rates.Add(rate1);
                context.Rates.Add(rate2);
                context.WalletFunds.Add(walletFunds1);
                context.WalletFunds.Add(walletFunds2);
                context.SaveChanges();

                IWalletRepository walletRepositor = new WalletRepository(context);
                List<Wallet> wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(2));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Currency, Is.EqualTo(baseCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Amount, Is.EqualTo(totalAmount));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Currency, Is.EqualTo(quoteCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Amount, Is.EqualTo(totalQuoteAmount));

                await walletRepositor.ExchangeFund(walletId,
                    new Transfer { Amount = quoteAmount, Currency = baseCurrency },
                    new Transfer { Amount = exchangedAmount, Currency = quoteCurrency });
                wallets = await walletRepositor.GetWallets();

                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.Count, Is.EqualTo(2));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Currency, Is.EqualTo(baseCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == baseCurrency).Amount, Is.EqualTo(totalAmount - quoteAmount));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Currency, Is.EqualTo(quoteCurrency));
                Assert.That(wallets.FirstOrDefault(x => x.Id == walletId)?.Funds.FirstOrDefault(x => x.Currency == quoteCurrency).Amount, Is.EqualTo(finalQuoteAmount));
            }
        }
    }
}