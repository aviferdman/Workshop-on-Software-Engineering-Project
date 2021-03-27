using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    class User
    {
        private State _state;
        private ShoppingCart _shoppingCart;
        private Guid _id;
        private Address _address;

        public User()
        {
            this._shoppingCart = new ShoppingCart();
            this.Id = new Guid();
        }

        public Guid Id { get => _id; set => _id = value; }
        public Address Address { get => _address; set => _address = value; }


        public bool CheckEnoughtCurrent(double paymentSum)
        {
            throw new NotImplementedException();
        }

        internal string getPhoneNumber()
        {
            throw new NotImplementedException();
        }

        internal Guid getBankAccount()
        {
            throw new NotImplementedException();
        }
    }
}
