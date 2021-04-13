﻿using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Service
{
    public class MarketService
    {
        private Market market;

        public MarketService()
        {
            market = Market.Instance;
        }

        //returns uniqe username for guest
        public string AddGuest()
        {
            return market.AddGuest();
        }

        public void RemoveGuest(String usrname)
        {
            market.RemoveGuest(usrname);
        }
        public void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode = false)
        {
            market.ActivateDebugMode(deliveryAdapter, paymentAdapter, debugMode);
        }

        //"Product added"
        public String AddProduct(ProductData product, Guid storeID, String username)
        {
            return market.AddProduct(product, storeID, username);
        }

        //"Product removed"
        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            return market.RemoveProduct(productName, storeID, username);
        }
        //"Product edited"
        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            return market.EditProduct(productName, details, storeID, username);
        }

        public String makeOwner(String assignee, Guid storeID, String assigner)
        {
            return market.makeOwner(assignee, storeID, assigner);
        }

        public String makeManager(String assignee, Guid storeID, String assigner)
        {
            return market.makeManager(assignee, storeID, assigner);
        }

        public StoreData CreateStore(string name, string username, int accountNumber, int branch, string state, string city, string street, string apartmentNum)
        {
            BankAccount bankAccount = new BankAccount(accountNumber, branch);
            Address address = new Address(state, city, street, apartmentNum);
            Store store = market.CreateStore(name, username, bankAccount, address);
            return new StoreData(store);
        }

        public bool PurchaseShoppingCart(string username, int accountNumber, int branch, string phone, string state, string city, string street, string apartmentNum)
        {
            BankAccount bankAccount = new BankAccount(accountNumber, branch);
            Address address = new Address(state, city, street, apartmentNum);
            return market.PurchaseShoppingCart(username, bankAccount, phone, address);
        }

        public ICollection<HistoryData> GetAllHistory(string username)
        {
            ICollection<IHistory> histories = market.GetAllHistory(username);
            ICollection<HistoryData> ret = new HashSet<HistoryData>();
            foreach (var his in histories)
            {
                ret.Add(new HistoryData(his));
            }
            return ret;
        }
        public HistoryData GetUserHistory(string username)
        {
            UserHistory history = market.GetUserHistory(username);
            return new HistoryData(history);
        }
        public HistoryData GetStoreHistory(string username, Guid storeId)
        {
            StoreHistory history = market.GetStoreHistory(username, storeId);
            return new HistoryData(history);
        }

        public ICollection<StoreData> findStoresByname(string name)
        {
            ICollection<Store> stores = market.GetStoresByName(name);
            ICollection<StoreData> dataStores = new LinkedList<StoreData>();
            foreach(Store s in stores)
            {
                dataStores.Add(new StoreData(s));
            }
            return dataStores;
        }

        public ICollection<ProductData> findProductsByStores(string name)
        {
            ICollection<Store> stores = market.GetStoresByName(name);
            ICollection<ProductData> products = new LinkedList<ProductData>();
            foreach (Store s in stores)
            {
                foreach(Product p in s.Products.Values)
                {
                    products.Add(new ProductData(p));
                }
            }
            return products;
        }

        public Dictionary<Guid, Dictionary<ProductData, int>> viewShoppingCart(string username)
        {
            ShoppingCart cart = (ShoppingCart) market.viewShoppingCart(username);
            if (cart == null)
                return null;
            Dictionary<Guid, Dictionary<ProductData, int>> dataCart = new Dictionary<Guid, Dictionary<ProductData, int>>();
            foreach (KeyValuePair<IStore, IShoppingBasket> entry in cart.Store_shoppingBasket)
            {
                Dictionary<ProductData, int> products = new Dictionary<ProductData, int>();
                foreach (KeyValuePair<Product, int> p in ((ShoppingBasket)entry.Value).Product_quantity){
                    products.Add(new ProductData(p.Key), p.Value);
                }
                dataCart.Add(((Store)entry.Key).Id, products);
            }
            return dataCart;
        }

        public Result<Dictionary<Guid, Dictionary<ProductData, int>>>  editShoppingCart(string username, Dictionary<Guid, string> products_removed, Dictionary<Guid, KeyValuePair<string, int>> products_added, Dictionary<Guid, KeyValuePair<string, int>> products_quan)
        {
            Result<IShoppingCart> res =market.editShoppingCart(username,  products_removed, products_added, products_quan);
            if (res.IsErr)
                return new Result<Dictionary<Guid, Dictionary<ProductData, int>>>(null, true, res.Mess);
            ShoppingCart cart = (ShoppingCart)res.Ret;
            if (cart == null)
                return null;
            Dictionary<Guid, Dictionary<ProductData, int>> dataCart = new Dictionary<Guid, Dictionary<ProductData, int>>();
            foreach (KeyValuePair<IStore, IShoppingBasket> entry in cart.Store_shoppingBasket)
            {
                Dictionary<ProductData, int> products = new Dictionary<ProductData, int>();
                foreach (KeyValuePair<Product, int> p in ((ShoppingBasket)entry.Value).Product_quantity)
                {
                    products.Add(new ProductData(p.Key), p.Value);
                }
                dataCart.Add(((Store)entry.Key).Id, products);
            }
            return new Result<Dictionary<Guid, Dictionary<ProductData, int>>>(dataCart, false,null);
        }

        public ICollection<ProductData> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            ICollection<Product> pro = market.findProducts(keyword, price_range_low, price_range_high, rating, category);
            ICollection<ProductData> products = new LinkedList<ProductData>();
                foreach (Product p in pro)
                {
                    products.Add(new ProductData(p));
                }
            return products;
        }



        public String DefineManagerPermissions(String manager, Guid storeID, String assigner, List<Permission> permissions)
        {
            return market.DefineManagerPermissions(manager, storeID, assigner, permissions);
        }
    }
}
