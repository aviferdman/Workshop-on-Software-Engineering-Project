using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.Histories;
using System.Threading.Tasks;
namespace TradingSystem.DAL
{
    class MarketContext : DbContext
    {
        public DbSet<DataUser> dataUsers { get; set; }
        public DbSet<ShoppingCart> membersShoppingCarts { get; set; }
        public DbSet<ShoppingBasket> membersShoppingBaskets { get; set; }
        public DbSet<MemberState> memberStates { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<AdministratorState> administratorStates { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Manager> managers { get; set; }
        public DbSet<TransactionStatus> transactionStatuses { get; set; }
        public DbSet<UserHistory> userHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite(@"Data Source=marketDB.db");

        public static MarketContext Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketContext>
       _lazy =
       new Lazy<MarketContext>
           (() => new MarketContext());
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataUser>()
                .HasKey(d => d.username);
        }
        public MarketContext(): base(){}

        public async Task<DataUser> GetDataUser(string username)
        {
           return await dataUsers.SingleAsync(u => u.username == username);
            
        }

        public async Task<bool> AddDataUser(DataUser u)
        {
            try
            {
                dataUsers.Add(u);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveDataUser(string userId)
        {
            try
            {
                DataUser u = new DataUser() { username = userId };
                dataUsers.Attach(u);
                dataUsers.Remove(u);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
