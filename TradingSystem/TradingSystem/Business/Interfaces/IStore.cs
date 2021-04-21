using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;
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

        public void AddRule(IRule rule);

        public void RemoveRule(IRule rule);

        public String AddProduct(Product product, string userID);

        public String RemoveProduct(Guid productID, string userID);

        public String EditProduct(Guid productID, Product editedProduct, string userID);

        public String AssignMember(string assigneeID, User assigner, string type);

        public void UpdateProduct(Product product);

        public void RemoveProduct(Product product);

        public void AddDiscount(Discount discount);

        public void RemoveDiscount(Guid discountId);

        public ICollection<IHistory> GetStoreHistory(string username);

        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> permissions);

        public String RemoveManager(String managerName, User assigner);
    }
}