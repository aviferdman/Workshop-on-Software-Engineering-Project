using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.Histories;
using System.Threading.Tasks;
using TradingSystem.Business.Payment;
using TradingSystem.Business.Delivery;
using TradingSystem.Business;
using TradingSystem.Business.Market.UserPackage;
using System.Linq;
using TradingSystem.Business.Market.Statuses;
using static TradingSystem.Business.Market.StoreStates.Manager;

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
        public DbSet<Appointer> appointers { get; set; }
        public DbSet<Founder> founders { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Store> stores { get; set; }

        public async Task AddHistory(TransactionStatus history)
        {
            transactionStatuses.Add(history);
            await SaveChangesAsync();
        }

        public DbSet<Manager> managers { get; set; }
        public DbSet<TransactionStatus> transactionStatuses { get; set; }
        public DbSet<DeliveryStatus> deliveryStatuses { get; set; }
        public DbSet<ProductHistoryData> historyDatas { get; set; }
        public DbSet<PurchasedProduct> purchasedProducts { get; set; }
        public DbSet<ProductInCart> productInCarts { get; set; }
        public DbSet<ProductHistoryData> productHistoryDatas { get; set; }

        public DbSet<PaymentStatus> paymentStatuses { get; set; }

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
            modelBuilder.Entity<MemberState>()
                .HasKey(d => d.username);
            modelBuilder.Entity<ShoppingCart>()
               .HasKey(s => s.username);
            modelBuilder.Entity<ShoppingBasket>()
               .HasKey(s => new { s.shoppingCart, s.store } );
            modelBuilder.Entity<Appointer>()
                .HasKey(a => new { a.s, a.m });
            modelBuilder.Entity<Manager>().HasKey(a => new { a.s, a.m });
            modelBuilder.Entity<Store>()
            .HasMany(b => b.Products)
            .WithOne();
            modelBuilder.Entity<Store>()
            .HasMany(b => b.managers)
            .WithOne();
            modelBuilder.Entity<Store>()
            .HasMany(b => b.owners)
            .WithOne();
            modelBuilder.Entity<Store>()
            .HasOne(b => b.founder)
            .WithOne();
            modelBuilder.Entity<Founder>()
            .HasOne(b => b.m)
            .WithMany().HasForeignKey(pt => pt.username);
            modelBuilder
                .Entity<Owner>()
            .HasOne(b => b.m)
            .WithMany()
            .HasForeignKey(pt => pt.username);
            modelBuilder.Entity<Owner>()
            .HasOne(b => b.appointer)
            .WithMany()
            .HasForeignKey(pt => pt.username);
            modelBuilder.Entity<Manager>()
            .HasOne(b => b.m)
            .WithMany()
            .HasForeignKey(pt => pt.username);
            modelBuilder.Entity<Manager>()
            .HasOne(b => b.appointer)
            .WithMany()
            .HasForeignKey(pt => pt.username);

        }
        public async Task<ICollection<TransactionStatus>> getAllHistories()
        {
            return await transactionStatuses
                                .Include(s => s._paymentStatus)
                                .Include(s => s._deliveryStatus)
                                .Include(s => s.productHistories).ThenInclude(h=> h.productId_quantity)
                                .ToListAsync();
        }

        public async Task<ICollection<TransactionStatus>> getUserHistories(string username)
        {
            return await transactionStatuses.Where(s=> s.username==username)
                                .Include(s => s._paymentStatus)
                                .Include(s => s._deliveryStatus)
                                .Include(s => s.productHistories).ThenInclude(h => h.productId_quantity)
                                .ToListAsync();
        }

        public async Task<ICollection<TransactionStatus>> getStoreHistories(Guid storeId)
        {
            return await transactionStatuses.Where(s => s.storeID == storeId)
                                .Include(s => s._paymentStatus)
                                .Include(s => s._deliveryStatus)
                                .Include(s => s.productHistories).ThenInclude(h => h.productId_quantity)
                                .ToListAsync();
        }

        

        public async Task<ICollection<Store>> getMemberStores(string usrname)
        {
            return await stores.Include(s => s.Products)
                                .Include(s => s.owners)
                                .Include(s => s.managers)
                                .Include(s => s.founder)
                                .Where(s=> s.founder.username==usrname||
                                            s.managers.Where(m=>m.username==usrname).Any()||
                                            s.owners.Where(m => m.username == usrname).Any())
                                .ToListAsync();
        }

        public MarketContext(): base(){}

        public async Task<DataUser> GetDataUser(string username)
        {
           return await dataUsers.SingleAsync(u => u.username == username);
            
        }

        public async Task AddNewMemberState(string username)
        {
            try
            {
                memberStates.Add(new MemberState(username));
                await SaveChangesAsync();
            }
            catch
            {
            }
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

        public async Task<MemberState> getMemberState(string usrname)
        {
            MemberState mem=await memberStates.SingleAsync(m => m.username == usrname);
            return mem;
        }

        public async Task<ShoppingCart> getShoppingCart(string usrname)
        {
            ShoppingCart sc= await membersShoppingCarts.SingleAsync(s => s.username == usrname);
            await Entry(sc).Collection(s => s.shoppingBaskets).LoadAsync();
            foreach(ShoppingBasket sb in sc.shoppingBaskets)
            {
                await Entry(sb).Collection(s => s._product_quantity).LoadAsync();
                await Entry(sb).Reference(s => s.store).LoadAsync();
                foreach (ProductInCart pr in sb._product_quantity)
                {
                   Entry(pr).Reference(p => p.product).Load();
                }
            }
            return sc;
        }

        public async Task AddNewShoppingCart(string username)
        {
            try
            {
                memberStates.Add(new MemberState(username));
                await SaveChangesAsync();
            }
            catch
            {
            }
        }

        public async Task<Store> getStore(Guid storeId)
        {
            Store sc = await stores.SingleAsync(s => s.sid == storeId);
            await Entry(sc).Reference(s => s.founder).LoadAsync();
            await Entry(sc.founder).Reference(s => s.m).LoadAsync();
            foreach (ShoppingBasket sb in sc.shoppingBaskets)
            {
                await Entry(sb).Collection(s => s._product_quantity).LoadAsync();
                await Entry(sb).Reference(s => s.store).LoadAsync();
                foreach (ProductInCart pr in sb._product_quantity)
                {
                    Entry(pr).Reference(p => p.product).Load();
                }
            }
            return sc;
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

        public async Task RemoveProduct(Product p)
        {
            try
            {
                products.Remove(p);
                await SaveChangesAsync();
            }
            catch
            {
            }
        }


    }
}
