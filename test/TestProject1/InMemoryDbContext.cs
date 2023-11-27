using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CurrencyWallet.Repository.UnitTest
{
    internal class InMemoryDbContext : CurrencyWalletDbContext
    {
        public InMemoryDbContext(DbContextOptions<CurrencyWalletDbContext> options) : base(options)
        {
        }

        public static InMemoryDbContext CreateInMemory(string dbname)
        {
            return new InMemoryDbContext(new DbContextOptionsBuilder<CurrencyWalletDbContext>()
                                .UseInMemoryDatabase(dbname)
                                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                .Options);
        }

    }
}
