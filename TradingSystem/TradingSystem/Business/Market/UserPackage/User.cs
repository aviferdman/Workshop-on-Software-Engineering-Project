﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Notifications;
using TradingSystem.Notifications;

namespace TradingSystem.Business.Market
{

    public class User
    {
        private State _state;
        private IShoppingCart _shoppingCart;
        private string username;
        private bool isLoggedIn;
        private ICollection<IHistory> userHistory;
        private Publisher publisher;
        private ICollection<NotificationSubscriber> subscribers;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart(this);
            this._state = new GuestState();
            this.username = username;
            this.isLoggedIn = false;
            this.UserHistory = new HashSet<IHistory>();
            InitNotifications();
        }

        private void InitNotifications()
        {
            this.Publisher = new Publisher(this);
            this.Subscribers = new HashSet<NotificationSubscriber>();
            NotificationSubscriber subscriber1 = new NotificationSubscriber(this, nameof(EventType.RegisterEvent));
            subscriber1.Subscribe(Publisher.Get(EventType.RegisterEvent));
            Subscribers.Add(subscriber1);
            NotificationSubscriber subscriber2 = new NotificationSubscriber(this, nameof(EventType.OpenStoreEvent));
            subscriber2.Subscribe(Publisher.Get(EventType.OpenStoreEvent));
            Subscribers.Add(subscriber2);
            NotificationSubscriber subscriber3 = new NotificationSubscriber(this, nameof(EventType.PurchaseEvent));
            subscriber3.Subscribe(Publisher.Get(EventType.PurchaseEvent));
            Subscribers.Add(subscriber3);
            NotificationSubscriber subscriber4 = new NotificationSubscriber(this, nameof(EventType.BecomeManagerEvent));
            subscriber4.Subscribe(Publisher.Get(EventType.BecomeManagerEvent));
            Subscribers.Add(subscriber4);
        }

        public User()
        {
            this._shoppingCart = new ShoppingCart(this);
            this._state = new GuestState();
            this.isLoggedIn = false;
            this.UserHistory = new HashSet<IHistory>();
            InitNotifications();
        }

        public State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public IShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }
        public ICollection<IHistory> UserHistory { get => userHistory; set => userHistory = value; }
        public Publisher Publisher { get => publisher; set => publisher = value; }
        public ICollection<NotificationSubscriber> Subscribers { get => subscribers; set => subscribers = value; }

        public void ChangeState(State state)
        {
            State = state;
        }

        public void UpdateProductInShoppingBasket(IStore store, Product product, int quantity)
        {
            IShoppingBasket shoppingBasket = ShoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        public Result<bool> PurchaseShoppingCart(PaymentMethod method, string phone, Address address)
        {
            
            BuyStatus buyStatus = ShoppingCart.Purchase(method, phone, address);
            //add information to history
            //empty the shopping cart after a successful purchase
            if (buyStatus.Status)
            {
                foreach (var p in buyStatus.PurchaseStatuses)
                {
                    var h = new UserHistory(p);
                    userHistory.Add(h);
                    HistoryManager.Instance.AddUserHistory(h);
                }
                this.ShoppingCart = new ShoppingCart(this);
            }
            return new Result<bool>(buyStatus.Status, !buyStatus.Status, "");
        }

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts()
        {
            return _shoppingCart.GetShopingCartProducts();
        }

        public ICollection<IHistory> GetUserHistory(string username)
        {
            return _state.GetUserHistory(username);
        }

       
    }
}
