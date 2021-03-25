using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Store
    {
        private ICollection<Product> products;
        private ICollection<User> users;
        private ICollection<ShoppingCart> shoppingCarts;
        private ICollection<ShoppingBasket> shoppingBaskets;
    }
}
