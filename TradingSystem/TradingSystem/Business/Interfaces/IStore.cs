using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public interface IStore : IComparable
    {
        public Guid GetId();
        public PurchaseStatus Purchase(IShoppingBasket shoppingBasket, string username, string clientPhone, Address clientAddress, PaymentMethod method, double paymentSum);

        public void CancelTransaction(Dictionary<Product, int> product_quantity);

        public double CalcPaySum(IShoppingBasket shoppingBasket);

        public double ApplyDiscounts(IShoppingBasket shoppingBasket);

        public bool CheckPolicy(IShoppingBasket shoppingBasket);

        public void AddRule(IRule rule);

        public void RemoveRule(IRule rule);

        public String AddProduct(Product product, string username);

        public String RemoveProduct(String productName, string username);

        public String EditProduct(String productName, Product editedProduct, string username);

        public String AssignMember(string username, User assigner, AppointmentType type);

        public void UpdateProduct(Product product);

        public void RemoveProduct(Product product);

        public void AddDiscount(Discount discount);

        public void RemoveDiscount(Guid discountId);

        public ICollection<IHistory> GetStoreHistory(string username);

        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> permissions);
    }
}