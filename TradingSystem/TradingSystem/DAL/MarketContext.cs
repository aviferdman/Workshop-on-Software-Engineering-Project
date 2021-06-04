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
        public DbSet<State> states { get; set; }
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
        public DbSet<PurchasedProduct> purchasedProducts { get; set; }
        public DbSet<ProductInCart> productInCarts { get; set; }
        public DbSet<ProductHistoryData> productHistoryDatas { get; set; }

        public DbSet<PaymentStatus> paymentStatuses { get; set; }
        public DbSet<Address> addresses { get; set; }

        public DbSet<Category> categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite(@ProxyMarketContext.conString);

        public static MarketContext Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketContext>
       _lazy =
       new Lazy<MarketContext>
           (() => new MarketContext());
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
            .HasMany(b => b.Products)
            .WithOne();
            modelBuilder.Entity<Store>()
            .HasMany(b => b.managers)
            .WithOne(m=>m.s);
            modelBuilder.Entity<Store>()
            .Ignore(b => b._bank);
            modelBuilder.Entity<Store>()
            .HasOne(b => b._address)
            .WithMany();
            modelBuilder.Entity<Store>()
            .HasMany(b => b.owners)
            .WithOne(m => m.s);
            modelBuilder.Entity<Founder>()
            .HasOne(f => f.s)
            .WithOne()
            .HasForeignKey<Founder>(f=>f.sid);
            modelBuilder.Entity<DataUser>()
                .HasKey(d => d.username);
            modelBuilder.Entity<State>()
                .HasKey(d => d.username);
            modelBuilder.Entity<DeliveryStatus>()
              .HasKey(s => s.PackageId);
            modelBuilder.Entity<PaymentStatus>()
              .HasKey(s => s.PaymentId);
            modelBuilder.Entity<ShoppingCart>()
               .HasKey(s => s.username);
            modelBuilder.Entity<Store>()
              .HasKey(s => s.sid);
            modelBuilder.Entity<Appointer>()
              .HasKey(s => new { s.sid, s.username });
            modelBuilder.Entity<Manager>()
              .HasKey(s => new { s.sid, s.username });
            modelBuilder.Entity<ShoppingBasket>()
              .HasKey(s=> s.id);
            modelBuilder.Entity<Category>()
               .HasKey(s => s.Name);
            modelBuilder.Entity<ShoppingCart>()
               .HasMany(s => s.shoppingBaskets).
               WithOne(s => s.shoppingCart);
            modelBuilder.Entity<ShoppingCart>()
               .Ignore(s => s.User1);
            modelBuilder.Entity<ShoppingBasket>()
            .HasMany(b => b._product_quantity)
            .WithOne();
            modelBuilder.Entity<ProductInCart>()
            .HasOne(b => b.product)
            .WithMany();
            modelBuilder.Entity<Owner>()
            .HasOne(b => b.appointer)
            .WithMany()
            .HasForeignKey(pt => pt.username);
           
            modelBuilder.Entity<Manager>()
            .HasOne(b => b.appointer)
            .WithMany()
            .HasForeignKey(pt => pt.username);
        }

        public async Task<ICollection<Product>> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            return await products.Where(p =>
                                    (p.Name.Contains(keyword) || p.category.Contains(keyword))
                                    &&(price_range_low==-1||price_range_low<=p.Price)
                                    &&(price_range_high == -1 || price_range_high >= p.Price)
                                    &&(rating == -1 || rating == p.Price)
                                    &&(category != null || category.Equals(p.category))).ToListAsync();
        }

        public async Task<Category> AddNewCategory(string category)
        {
            Category c;
            if (categories.Where(c => c.Name.Equals(category)).Any())
            {
                c = categories.Single(c => c.Name.Equals(category));
            }
            else
            {
                c = new Category(category);
                await categories.AddAsync(c);
            }
            return c;
        }

        public void findStoreProduct(out Store found, out Product p, Guid pid)
        {
            Store sc =  stores.Include(s=> s._products)
                .Single(s => s._products.Where(p=>p.Id.Equals(pid)).Any());
            Entry(sc).Reference(s => s.founder).Load();
            Entry(sc.founder).Reference(s => s.m).Load();
            Entry(sc).Reference(s => s._address).Load();
            Entry(sc).Reference(s => s._bank).Load();
            Entry(sc).Collection(s => s.managers).Load();
            Entry(sc).Collection(s => s.owners).Load();
            foreach (Manager m in sc.managers)
            {
                Entry(m).Reference(s => s.m).Load();
            }
            foreach (Owner m in sc.owners)
            {
                Entry(m).Reference(s => s.m).Load();
            }
            found = sc;
            p = sc.Products.Single(p => p.Id.Equals(pid));
        }

        public async Task<ICollection<Store>> GetStoresByName(string name)
        {
            ICollection<Store>  sc =  stores.Where(s => s.name.ToLower().Contains(name)).ToList();
            
            foreach (Store s in sc)
            {
                await Entry(s).Collection(s => s._products).LoadAsync();
            }
            return sc;
        }

        public async Task removeOwner(Owner ownerToRemove)
        {
            try
            {
                owners.Remove(ownerToRemove);
                await SaveChangesAsync();
            }
            catch
            {
            }
        }

        public async Task AddStore(Store store)
        {
            await stores.AddAsync(store);
            await SaveChangesAsync();
        }

        public void removeProductFromCart(ProductInCart productInCart)
        {
            try
            {
                productInCarts.Remove(productInCart);
            }
            catch
            {
            }
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
            return await transactionStatuses.Where(s=> s.username.Equals(username))
                                .Include(s => s._paymentStatus)
                                .Include(s => s._deliveryStatus)
                                .Include(s => s.productHistories).ThenInclude(h => h.productId_quantity)
                                .ToListAsync();
        }

        public async Task<ICollection<TransactionStatus>> getStoreHistories(Guid storeId)
        {
            return await transactionStatuses.Where(s => s.storeID.Equals(storeId))
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
                                .Where(s=> s.founder.username.Equals(usrname)||
                                            s.managers.Where(m=>m.username.Equals(usrname)).Any()||
                                            s.owners.Where(m => m.username.Equals(usrname)).Any())
                                .ToListAsync();
        }

        public MarketContext(): base(){}

        public async Task<DataUser> GetDataUser(string username)
        {
           return await dataUsers.SingleAsync(u => u.username.Equals(username));
            
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
            MemberState mem=await memberStates.SingleAsync(m => m.username.Equals(usrname));
            return mem;
        }

        public async Task<ShoppingCart> getShoppingCart(string usrname)
        {
            ShoppingCart sc= await membersShoppingCarts.SingleAsync(s => s.username.Equals(usrname));
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
            Store sc = await stores.SingleAsync(s => s.sid.Equals(storeId));
            await Entry(sc).Reference(s => s.founder).LoadAsync();
            await Entry(sc.founder).Reference(s => s.m).LoadAsync();
            await Entry(sc).Reference(s => s._address).LoadAsync();
            await Entry(sc).Reference(s => s._bank).LoadAsync();
            await Entry(sc).Collection(s => s._products).LoadAsync();
            await Entry(sc).Collection(s => s.managers).LoadAsync();
            await Entry(sc).Collection(s => s.owners).LoadAsync();
            foreach(Manager m in sc.managers)
            {
                await Entry(m).Reference(s => s.m).LoadAsync();
            }
            foreach (Owner m in sc.owners)
            {
                await Entry(m).Reference(s => s.m).LoadAsync();
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

        public async Task removeManager(Manager manager)
        {
            try
            {
                managers.Remove(manager);
                await SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}
