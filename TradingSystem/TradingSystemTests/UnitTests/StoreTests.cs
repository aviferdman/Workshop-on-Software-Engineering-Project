using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StoreTests
    {
        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckLegalPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            Guid clienId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            //Mock<BankAccount> bankAccount = new Mock<BankAccount>();
            //bankAccount.Setup(ba => ba.CheckEnoughtCurrent(It.IsAny<double>())).Returns(true);
            BankAccount bankAccount = new BankAccount(1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = store.Purchase(product_quantity, clienId, clientPhone, address, bankAccount, paySum);
            Assert.AreEqual(true, purchaseStatus.TransactionStatus.Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckNotEnoghtProductQuantityPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            Guid clienId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            //Mock<BankAccount> bankAccount = new Mock<BankAccount>();
            //bankAccount.Setup(ba => ba.CheckEnoughtCurrent(It.IsAny<double>())).Returns(true);
            BankAccount bankAccount = new BankAccount(1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 11);
            product_quantity.Add(product2, 22);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = store.Purchase(product_quantity, clienId, clientPhone, address, bankAccount, paySum);
            Assert.AreEqual(false, purchaseStatus.PreConditions);

        }


        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyDiscountsNoDiscounts()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 5);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Assert.AreEqual(0,store.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyTwoRelevantDiscounts()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 30);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Discount discount1 = new Discount(100);
            discount1.AddRule(new Rule(MoreThan10Products));
            Discount discount2 = new Discount(200);
            discount1.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(discount1);
            store.AddDiscount(discount2);
            Assert.AreEqual(200, store.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyOneRelevantDiscount()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 19);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Discount discount1 = new Discount(100);
            discount1.AddRule(new Rule(MoreThan10Products));
            Discount discount2 = new Discount(200);
            discount2.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(discount1);
            store.AddDiscount(discount2);
            Assert.AreEqual(100, store.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckValidAddProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Guid founderID = Guid.NewGuid();
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Product added");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductUnauthorizedUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            Guid user = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidPrice()
        {
            Product product1 = new Product("1", 10, 10, -10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidName()
        {
            Product product1 = new Product("", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckValidRemoveProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", founderID), "Product removed");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Guid userID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", userID), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Mock<IManager> imanager = new Mock<IManager>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(false);
            IManager manager = imanager.Object;
            Guid founderID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", managerID), "No Permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckValidEditProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, founderID), "Product edited");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        public void CheckEditUnavailablwProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Guid founderID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("2", product2, founderID), "Product not in the store");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckEditNoPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(true);
            IFounder founder = ifounder.Object;
            Mock<IManager> imanager = new Mock<IManager>();
            ifounder.Setup(f => f.GetPermission(It.IsAny<Permission>())).Returns(false);
            IManager manager = imanager.Object;
            Guid founderID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, managerID), "No Permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckValidMakeOwner()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IOwner> iowner = new Mock<IOwner>();
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Returns(iowner.Object);
            IFounder assigner = ifounder.Object;
            User user = new User("assigner");
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Owner), "Success");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IOwner> iowner = new Mock<IOwner>();
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Returns(iowner.Object);
            IFounder assigner = ifounder.Object;
            IOwner owner = iowner.Object;
            User user = new User("assigner");
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            personnel.TryAdd(assigneeID, owner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Owner), "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerInvalidAssigner()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IManager> imanager = new Mock<IManager>();
            imanager.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Throws(new UnauthorizedAccessException());
            IManager assigner = imanager.Object;
            User uAssigner = new User("manager");
            Guid assignee = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(uAssigner.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assignee, uAssigner, AppointmentType.Owner), "Invalid assigner");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckValidMakeManager()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IOwner> iowner = new Mock<IOwner>();
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Returns(iowner.Object);
            IFounder assigner = ifounder.Object;
            User user = new User("assigner");
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Manager), "Success");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeManagerAlreadyAssigned()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IOwner> iowner = new Mock<IOwner>();
            Mock<IFounder> ifounder = new Mock<IFounder>();
            ifounder.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Returns(iowner.Object);
            IFounder assigner = ifounder.Object;
            IOwner owner = iowner.Object;
            User user = new User("assigner");
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            personnel.TryAdd(assigneeID, owner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Manager), "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeManagerInvalidAssigner()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Mock<IManager> imanager = new Mock<IManager>();
            imanager.Setup(f => f.AddAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentType>())).Throws(new UnauthorizedAccessException());
            IManager assigner = imanager.Object;
            User uAssigner = new User("manager");
            Guid assignee = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(uAssigner.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assignee, uAssigner, AppointmentType.Manager), "Invalid assigner");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        public void CheckValidDefinePermissions()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Guid assignerID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            Founder assigner = new Founder(assignerID);
            Manager manager = new Manager(managerID, assigner);
            manager.Appointer = assigner;
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(assignerID, assigner);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);

            Assert.AreEqual(store.DefineManagerPermissions(managerID, assignerID, permissions), "Success");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        public void CheckDefinePermissionsInvalidAssigner()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Guid user1ID = Guid.NewGuid();
            Guid user2ID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            Founder user1 = new Founder(user1ID);
            Founder user2 = new Founder(user2ID);
            Manager manager = new Manager(managerID, user1);
            manager.Appointer = user1;
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user1ID, user1);
            personnel.TryAdd(user2ID, user2);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);

            Assert.AreEqual(store.DefineManagerPermissions(managerID, user2ID, permissions), "The manager must be appointed by the user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        public void CheckDefinePermissionsNoManager()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            Guid assignerID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            Founder assigner = new Founder(assignerID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(assignerID, assigner);
            store.Personnel = personnel;
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);

            Assert.AreEqual(store.DefineManagerPermissions(managerID, assignerID, permissions), "Manager doesn't exist");
        }

        public bool MoreThan10Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach(KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 10;
        }

        public bool MoreThan20Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 20;
        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

        //END OF UNIT TESTS
    }
}
