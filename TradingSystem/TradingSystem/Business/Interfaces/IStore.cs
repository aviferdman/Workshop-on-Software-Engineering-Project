using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public interface IStore : IComparable
    {
        public Guid GetId();
        public bool isStaff(string username);
        public PurchaseStatus Purchase(IShoppingBasket shoppingBasket, string clientPhone, Address clientAddress, PaymentMethod method);

        public void CancelTransaction(Dictionary<Product, int> product_quantity);

        public double CalcPaySum(IShoppingBasket shoppingBasket);

        public double ApplyDiscounts(IShoppingBasket shoppingBasket);

        public bool CheckPolicy(IShoppingBasket shoppingBasket);

        public void SetPolicy(Policy policy);

        public Policy GetPolicy();

        public Guid AddRule(IRule rule);

        public void RemoveRule();

        public String AddProduct(Product product, string userID);

        public String RemoveProduct(Guid productID, string userID);

        public String EditProduct(Guid productID, Product editedProduct, string userID);

        public String AssignMember(string assigneeID, User assigner, string type);

        public void UpdateProduct(Product product);

        public void RemoveProduct(Product product);

        public Guid AddDiscount(Discount discount);

        public Guid RemoveDiscount(Guid discountId);

        public ICollection<IHistory> GetStoreHistory(string username);

        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> permissions);

        public String RemoveManager(String managerName, String assigner);

        public String RemoveOwner(String ownerName, String assigner);

        public String getInfo(String username);

        public String getInfoSpecific(String workerName, String username);

        public IRule GetRuleById(Guid ruleId);

        public Discount GetDiscountById(Guid discountId);
        public IRule GetDiscountRuleById(Guid ruleId);
    }
}