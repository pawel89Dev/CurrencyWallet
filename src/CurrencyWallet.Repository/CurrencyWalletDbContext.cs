using CurrencyWallet.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CurrencyWallet.Repository
{
    public class CurrencyWalletDbContext : DbContext
    {
        public DbSet<WalletEntity> Wallets { get; set; }
        public DbSet<RateEntity> Rates { get; set; }
        public DbSet<WalletFundsEnitity> WalletFunds{ get; set; }

        public string DbPath { get; }

        public CurrencyWalletDbContext(DbContextOptions<CurrencyWalletDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<WalletFundsEnitity>().HasKey(wf => new { wf.WalletId, wf.RateId});

        }

    }
}