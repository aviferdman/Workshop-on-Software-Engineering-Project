﻿using System;
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
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Business.Market.BidsPackage;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DbSet<Statistics> statistics { get; set; }
        
        public DbSet<AdministratorState> administratorStates { get; set; }
        public DbSet<Appointer> appointers { get; set; }

        public  Statistics getStatis(DateTime date)
        {
            Statistics s;
            try{
                s= statistics.Single(s => s.date.Day.Equals(date.Day));
            }
            catch(Exception e)
            {
                s = new Statistics();
                s.date = date;
                statistics.Add(s);
                try
                {
                    SaveChanges();
                }
                catch(Exception ex)
                {
                    return null;
                }
                
            }
            return s;
        }

        public DbSet<Founder> founders { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Store> stores { get; set; }
        public DbSet<BidsManager> BidsManager { get; set; }
        public DbSet<BidState> BidStates { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<PurchasePolicy> PurchasePolicy { get; set; }
        public async Task AddHistory(TransactionStatus history)
        {
            transactionStatuses.Add(history);
            await SaveChangesAsync();
        }

        public DbSet<Manager> managers { get; set; }
        public DbSet<Prem> Prem { get; set; }
        public DbSet<TransactionStatus> transactionStatuses { get; set; }
        public DbSet<DeliveryStatus> deliveryStatuses { get; set; }
        public DbSet<PurchasedProduct> purchasedProducts { get; set; }
        public DbSet<ProductInCart> productInCarts { get; set; }
        public DbSet<ProductHistoryData> productHistoryDatas { get; set; }

        public DbSet<PaymentStatus> paymentStatuses { get; set; }
        public DbSet<Address> addresses { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<MarketRulesRequestType1> marketRulesRequestType1 { get; set; }
        public DbSet<MarketRulesRequestType2> marketRulesRequestType2 { get; set; }
        public DbSet<MarketRulesRequestType3> marketRulesRequestType3 { get; set; }
        public DbSet<MarketRulesRequestType4> marketRulesRequestType4 { get; set; }
        public DbSet<MarketRulesRequestType5> marketRulesRequestType5 { get; set; }
        public DbSet<MarketRulesRequestType6> marketRulesRequestType6 { get; set; }
        public DbSet<MarketRulesRequestType7> marketRulesRequestType7 { get; set; }
        public DbSet<MarketRulesRequestType8> marketRulesRequestType8 { get; set; }

        internal void EmptyShppingCart(string username)
        {
            productInCarts.RemoveRange(membersShoppingBaskets.Include(s => s.ShoppingCart).Where(s => s.ShoppingCart.username.Equals(username)).SelectMany(s=>s._product_quantity).ToList()) ;
            membersShoppingBaskets.RemoveRange(membersShoppingBaskets.Include(s=>s.ShoppingCart).Where(s => s.ShoppingCart.username.Equals(username)));
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (ProxyMarketContext.conType.Equals("SQLite"))
            {
                options.UseSqlite(@ProxyMarketContext.conString);
            }
            else
            {
                options.UseSqlServer(@ProxyMarketContext.conString);
            }
        }

        public static MarketContext Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketContext>
       _lazy =
       new Lazy<MarketContext>
           (() => new MarketContext());
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
            .HasMany(b => b._products)
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
            modelBuilder.Entity<BidState>().HasKey(s => s.id);
            
            modelBuilder.Entity<Store>().HasOne(s => s.founder).WithOne(f=>f.s);
            modelBuilder.Entity<Store>().HasOne(s => s.BidsManager).WithOne(f => f.s).HasForeignKey<BidsManager>("sid").OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BidsManager>().HasMany(s => s.bidsState).WithOne().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BidState>().HasOne(s => s.Bid).WithOne().HasForeignKey<Bid>("stateId").OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DataUser>()
                .HasKey(d => d.username);
            modelBuilder.Entity<State>()
                .HasKey(d => d.username);
            modelBuilder.Entity<DeliveryStatus>()
              .HasKey(s => s.PackageId);
            modelBuilder.Entity<PaymentStatus>()
              .HasKey(s => s.PaymentId);
            modelBuilder.Entity<Statistics>()
             .HasKey(s => s.date);
            
            modelBuilder.Entity<ShoppingCart>()
               .HasKey(s => s.username);
            modelBuilder.Entity<Store>()
              .HasKey(s => s.sid);
            modelBuilder.Entity<Store>()
              .Property(s=>s.sid).ValueGeneratedNever();
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
               WithOne(s => s.ShoppingCart);
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
            .WithMany().HasForeignKey("appointerID");
           
            modelBuilder.Entity<Manager>()
            .HasOne(b => b.appointer)
            .WithMany().HasForeignKey("appointerID");
            modelBuilder.Entity<MarketRulesRequestType1>().Property(x => x.id).ValueGeneratedNever();
            modelBuilder.Entity<MarketRulesRequestType2>().Property(x => x.id).ValueGeneratedNever();
            modelBuilder.Entity<MarketRulesRequestType3>().Property(x => x.id).ValueGeneratedNever();
            modelBuilder.Entity<MarketRulesRequestType4>().Property(x => x.id).ValueGeneratedNever();
            modelBuilder.Entity<MarketRulesRequestType5>().Property(x => x.id).ValueGeneratedNever();
            modelBuilder.Entity<MarketRulesRequestType6>().Property(x => x.id).ValueGeneratedNever();
        }

        public async Task<ICollection<Product>> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            return  products.Where(p =>
                                    (p._name.Contains(keyword) || p.category.Contains(keyword))
                                    &&(price_range_low==-1||price_range_low<=p._price)
                                    &&(price_range_high == -1 || price_range_high >= p._price)
                                    &&(rating == -1 || rating == p.rating)
                                    &&(category == null || category.Equals(p.category))).ToList();
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
                await SaveChangesAsync();
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
            Entry(sc).Reference(s => s.BidsManager).Load();
            Entry(sc).Reference(s => s.purchasePolicy).Load();
            Entry(sc.BidsManager).Collection(s => s.bidsState).Load();
            foreach (BidState state in sc.BidsManager.bidsState)
            {
                Entry(state).Reference(s => s.Bid).Load();
            }
            Entry(sc.purchasePolicy).Collection(s => s.availablePurchaseKinds).Load();
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

        internal void tearDown()
        {
            try
            {
                dataUsers.RemoveRange(dataUsers.ToList());
                states.RemoveRange(states.ToList());
                statistics.RemoveRange(statistics.ToList());
                productInCarts.RemoveRange(productInCarts.ToList());
                membersShoppingBaskets.RemoveRange(membersShoppingBaskets.ToList());
                membersShoppingCarts.RemoveRange(membersShoppingCarts.ToList());
                appointers.RemoveRange(appointers.ToList());
                Prem.RemoveRange(Prem.ToList());
                managers.RemoveRange(managers.ToList());
                products.RemoveRange(products.ToList());
                addresses.RemoveRange(addresses.ToList());
                BidsManager.RemoveRange(BidsManager.ToList());
                transactionStatuses.RemoveRange(transactionStatuses.ToList());
                deliveryStatuses.RemoveRange(deliveryStatuses.ToList());
                purchasedProducts.RemoveRange(purchasedProducts.ToList());
                productHistoryDatas.RemoveRange(productHistoryDatas.ToList());
                paymentStatuses.RemoveRange(paymentStatuses.ToList());
                stores.RemoveRange(stores.ToList());
                PurchasePolicy.RemoveRange(PurchasePolicy.ToList());
                categories.RemoveRange(categories.ToList());
                marketRulesRequestType1.RemoveRange(marketRulesRequestType1.ToList());
                marketRulesRequestType2.RemoveRange(marketRulesRequestType2.ToList());
                marketRulesRequestType3.RemoveRange(marketRulesRequestType3.ToList());
                marketRulesRequestType4.RemoveRange(marketRulesRequestType4.ToList());
                marketRulesRequestType5.RemoveRange(marketRulesRequestType5.ToList());
                marketRulesRequestType6.RemoveRange(marketRulesRequestType6.ToList());
                marketRulesRequestType7.RemoveRange(marketRulesRequestType7.ToList());
                marketRulesRequestType8.RemoveRange(marketRulesRequestType8.ToList());
                SaveChanges();
            }
            catch (Exception e)
            {

            }
            
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
            catch(Exception e)
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
            catch(Exception e)
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
                membersShoppingCarts.Add(new ShoppingCart(username));
                await SaveChangesAsync();
            }
            catch(Exception e)
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
            await Entry(sc).Reference(s => s.BidsManager).LoadAsync();
            await Entry(sc).Reference(s => s.purchasePolicy).LoadAsync();
            await Entry(sc).Collection(s => s._products).LoadAsync();
            Entry(sc.BidsManager).Collection(s => s.bidsState).Load();
            foreach(BidState state in sc.BidsManager.bidsState)
            {
                Entry(state).Reference(s => s.Bid).Load();
            }
            Entry(sc.purchasePolicy).Collection(s => s.availablePurchaseKinds).Load();
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
            await getDiscoutsPolicies(storeId);
            return sc;
        }

        private async Task getDiscoutsPolicies(Guid storeId)
        {
            ICollection<MarketRulesRequestType1> type1 = await marketRulesRequestType1.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType2> type2 = await marketRulesRequestType2.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType3> type3 = await marketRulesRequestType3.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType4> type4 = await marketRulesRequestType4.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType5> type5 = await marketRulesRequestType5.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType6> type6 = await marketRulesRequestType6.Where(r => r.storeId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType7> type7 = await marketRulesRequestType7.Where(r => r.StoreId.Equals(storeId)).ToListAsync();
            ICollection<MarketRulesRequestType8> type8 = await marketRulesRequestType8.Where(r => r.StoreId.Equals(storeId)).ToListAsync();
            List<MarketRuleRequest> ruleRequests = new List<MarketRuleRequest>();
            ruleRequests.AddRange(type1);
            ruleRequests.AddRange(type2);
            ruleRequests.AddRange(type3);
            ruleRequests.AddRange(type4);
            ruleRequests.AddRange(type5);
            ruleRequests.AddRange(type7);
            ruleRequests.AddRange(type8);
            ruleRequests.AddRange(type6);
            ruleRequests.OrderBy(r => r.getCounter());
            foreach(MarketRuleRequest r in ruleRequests)
            {
                await r.ActivateFunction();
            }
        }

        public async Task AddProduct(Product product)
        {
            products.Add(product);
            await SaveChangesAsync();
        }

        public async Task<int> getRuleCounter()
        {
            ICollection<MarketRulesRequestType1> type1 = await marketRulesRequestType1.ToListAsync();
            ICollection<MarketRulesRequestType2> type2 = await marketRulesRequestType2.ToListAsync();
            ICollection<MarketRulesRequestType3> type3 = await marketRulesRequestType3.ToListAsync();
            ICollection<MarketRulesRequestType4> type4 = await marketRulesRequestType4.ToListAsync();
            ICollection<MarketRulesRequestType5> type5 = await marketRulesRequestType5.ToListAsync();
            ICollection<MarketRulesRequestType6> type6 = await marketRulesRequestType6.ToListAsync();
            List<MarketRuleRequest> ruleRequests = new List<MarketRuleRequest>();
            ruleRequests.AddRange(type1);
            ruleRequests.AddRange(type2);
            ruleRequests.AddRange(type3);
            ruleRequests.AddRange(type4);
            ruleRequests.AddRange(type5);
            ruleRequests.AddRange(type6);
            if (ruleRequests.Count == 0)
                return 0;
            return ruleRequests.Max(m => m.getCounter());
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

        public  void removeManager(Manager manager)
        {
            try
            {
                Prem.RemoveRange(manager.store_permission);
                managers.Remove(manager);
                SaveChanges();
            }
            catch(Exception e)
            {
            }
        }

        public async Task AddRequestType1(MarketRulesRequestType1 req)
        {
            await marketRulesRequestType1.AddAsync(req);
            await SaveChangesAsync();
        }
        public async Task AddRequestType7(MarketRulesRequestType7 req)
        {
            await marketRulesRequestType7.AddAsync(req);
            await SaveChangesAsync();
        }
        public async Task AddRequestType8(MarketRulesRequestType8 req)
        {
            await marketRulesRequestType8.AddAsync(req);
            await SaveChangesAsync();
        }

        public async Task AddRequestType2(MarketRulesRequestType2 req)
        {
            await marketRulesRequestType2.AddAsync(req);
            await SaveChangesAsync();
        }

        public async Task AddRequestType3(MarketRulesRequestType3 req)
        {
            await marketRulesRequestType3.AddAsync(req);
            await SaveChangesAsync();
        }

        public async Task AddRequestType4(MarketRulesRequestType4 req)
        {
            await marketRulesRequestType4.AddAsync(req);
            await SaveChangesAsync();
        }

        public async Task AddRequestType5(MarketRulesRequestType5 req)
        {
            await marketRulesRequestType5.AddAsync(req);
            await SaveChangesAsync();
        }

        public async Task AddRequestType6(MarketRulesRequestType6 req)
        {
            await marketRulesRequestType6.AddAsync(req);
            await SaveChangesAsync();
        }


    }
}
