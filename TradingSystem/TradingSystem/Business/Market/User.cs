﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    class User
    {
        private State _state;
        private ShoppingCart _shoppingCart;
        private string _id;
        private Address _address;

        public string Id { get => _id; set => _id = value; }
        internal Address Address { get => _address; set => _address = value; }


        public bool CheckEnoughtCurrent(double paymentSum)
        {
            throw new NotImplementedException();
        }

        internal string getPhoneNumber()
        {
            throw new NotImplementedException();
        }

        internal string getBankAccount()
        {
            throw new NotImplementedException();
        }
    }
}
