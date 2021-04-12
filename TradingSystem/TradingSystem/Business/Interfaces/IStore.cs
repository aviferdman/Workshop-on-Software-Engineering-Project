﻿using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public interface IStore : IComparable
    {
        public Guid GetId();
        public PurchaseStatus Purchase(Dictionary<Product, int> product_quantity, Guid clientId, string clientPhone, Address clientAddress, BankAccount clientBankAccount, double paymentSum);

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

        public String DefineManagerPermissions(Guid managerID, Guid assignerID, List<Permission> permissions);
    }
}