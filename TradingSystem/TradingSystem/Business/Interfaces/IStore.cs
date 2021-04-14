using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public interface IStore : IComparable
    {
        public Guid GetId();
        public PurchaseStatus Purchase(IShoppingBasket shoppingBasket, Guid clientId, string clientPhone, Address clientAddress, PaymentMethod method, double paymentSum);

        public void CancelTransaction(Dictionary<Product, int> product_quantity);

        public double CalcPaySum(IShoppingBasket shoppingBasket);

        public double ApplyDiscounts(IShoppingBasket shoppingBasket);

        public bool CheckPolicy(IShoppingBasket shoppingBasket);

        public void AddRule(IRule rule);

        public void RemoveRule(IRule rule);

        public String AddProduct(Product product, Guid userID);

        public String RemoveProduct(String productName, Guid userID);

        public String EditProduct(String productName, Product editedProduct, Guid userID);

        public String AssignMember(Guid assigneeID, User assigner, AppointmentType type);

        public void UpdateProduct(Product product);

        public void RemoveProduct(Product product);

        public void AddDiscount(Discount discount);

        public void RemoveDiscount(Guid discountId);

        public ICollection<IHistory> GetStoreHistory(Guid userID);

        public String DefineManagerPermissions(Guid managerID, Guid assignerID, List<Permission> permissions);
    }
}