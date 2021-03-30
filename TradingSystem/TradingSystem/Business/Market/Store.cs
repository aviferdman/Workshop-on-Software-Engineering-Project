﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    public class Store
    {
        private ICollection<Product> _products;
        private ICollection<TransactionStatus> _transactionsHistory;
        private BankAccount _bank;
        private Guid _id;
        private string name;
        private ICollection<Discount> _discounts;
        private Policy _policy;
        private Address _address;
        private static Transaction _transaction = Transaction.Instance;
        private object _lock;

        internal ICollection<Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        internal ICollection<TransactionStatus> TransactionsHistory { get => _transactionsHistory; set => _transactionsHistory = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => name; set => name = value; }
        internal BankAccount Bank { get => _bank; set => _bank = value; }
        public ICollection<Discount> Discounts { get => _discounts; set => _discounts = value; }

        public Store(string name, BankAccount bank, Address address)
        {
            this.name = name;
            this._products = new HashSet<Product>();
            this._transactionsHistory = new HashSet<TransactionStatus>();
            this._lock = new object();
            this.Discounts = new HashSet<Discount>();
            this._id = new Guid();
            this._policy = new Policy();
            this._bank = bank;
            this._address = address;
        }

        public PurchaseStatus Purchase(Dictionary<Product, int> product_quantity, Guid clientId, string clientPhone, Address clientAddress, BankAccount clientBankAccount, double paymentSum)
        {
            bool enoughtQuantity, enoughtCurrent;
            TransactionStatus transactionStatus;
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            enoughtCurrent = clientBankAccount.CheckEnoughtCurrent(paymentSum);
            //pre-conditions not legal
            if (!enoughtCurrent) return new PurchaseStatus(false, null, _id, product_quantity);

            lock (_lock)
            {
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, _id, product_quantity);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = _transaction.ActivateTransaction(clientId, clientPhone, weight, _address, clientAddress, clientBankAccount, _id, _bank, paymentSum);
            _transactionsHistory.Add(transactionStatus);
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            return new PurchaseStatus(true, transactionStatus, _id, product_quantity);
        }

        public void CancelTransaction(Dictionary<Product, int> product_quantity)
        {
            UpdateQuantities(product_quantity, false);
        }

        public double CalcPaySum(ShoppingBasket shoppingBasket)
        {
            double discount = ApplyDiscounts(shoppingBasket);
            double cost = shoppingBasket.CalcCost();
            return cost - discount;
        }

        public double ApplyDiscounts(ShoppingBasket shoppingBasket)
        {
            var availableDiscounts = Discounts.Select(d=>d.ApplyDiscounts(shoppingBasket.Product_quantity));
            //chose the max value of an available discount
            try
            {
                return availableDiscounts.Max();
            }
            catch   // no discount is available
            {
                return 0;
            }
        }

        //use case 12 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/75
        public bool CheckPolicy(ShoppingBasket shoppingBasket)
        {
            return Policy.Check(shoppingBasket.Product_quantity);
        }

        public void AddRule(Rule rule)
        {
            _policy.AddRule(rule);
        }

        public void RemoveRule(Rule rule)
        {
            _policy.RemoveRule(rule);
        }

        public void UpdateProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p!=null){    //remove old product if exists            {
                _products.Remove(p);
            }   
            
            _products.Add(product); //add the new product
           
        }

        public void RemoveProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p != null)
            {    //remove old product if exists            {
                _products.Remove(p);
            }
        }

        public void AddDiscount(Discount discount)
        {
            Discounts.Add(discount);
        }

        public void RemoveDiscount(Guid discountId)
        {
            Discount d = GetDiscountById(discountId);
            if (d != null)
            {    //remove old discount if exists            {
                Discounts.Remove(d);
            }
        }

        private void UpdateQuantities(Dictionary<Product, int> product_quantity, bool subOrAdd = true)
        {
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                Product product = GetProductById(p_q.Key.Id);
                if (subOrAdd)
                {   
                    product.Quantity = product.Quantity - p_q.Value;
                }
                else
                {
                    product.Quantity = product.Quantity + p_q.Value;
                }
            }
        }

        private bool EnoughQuantity(Dictionary<Product, int> product_quantity)
        {
            bool enoughtQuantity = true;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                Guid productId = p_q.Key.Id;
                Product p = GetProductById(productId);
                enoughtQuantity = enoughtQuantity && p != null && p.Quantity >= p_q.Value;
            }
            return enoughtQuantity;
        }

        private Product GetProductById(Guid productId)
        {
            IEnumerable<Product> products = Products.Where(product => product.Id.Equals(productId));
            return products.FirstOrDefault();
        }

        private Discount GetDiscountById(Guid discountId)
        {
            IEnumerable<Discount> discounts = Discounts.Where(discount => discount.Id.Equals(discountId));
            return discounts.FirstOrDefault();
        }

    }
}
