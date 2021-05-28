using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market
{
    public class ShoppingBasket :IComparable
    {
        public Guid id { get; set; }
        public HashSet<ProductInCart> _product_quantity { get; set; }
        public  ShoppingCart shoppingCart { get; set; }
        public Store store { get; set; }

        public ShoppingBasket(ShoppingCart shoppingCart, Store store)
        {
            this.ShoppingCart = shoppingCart;
            this.Store = store;
            this._product_quantity = new HashSet<ProductInCart>();
        }

        public ShoppingBasket(ShoppingBasket s, ShoppingCart shoppingCart)
        {
            this.store = s.store;
            this.shoppingCart = shoppingCart;
            this._product_quantity =new  HashSet<ProductInCart>();
            foreach(ProductInCart p in s.Product_quantity)
            {
                this.Product_quantity.Add(new ProductInCart(p.product, p.quantity));
            }
        }

        public ShoppingBasket()
        {
        }

        public HashSet<ProductInCart> Product_quantity { get => _product_quantity; set => _product_quantity = value; }
        public ShoppingCart ShoppingCart { get => shoppingCart; set => shoppingCart = value; }
        public Store Store { get => store; set => store = value; }

        public virtual bool IsEmpty()
        {
            var possitiveQuantities = _product_quantity.Where(p_q => p_q.quantity > 0);
            return !possitiveQuantities.Any();
        }

        public virtual async Task<string> addProduct(Product p, int q)
        {
            if (_product_quantity.Where(pr=> pr.product.Id.Equals(p.Id)).Any())
                return "product is already in shopping basket";
            _product_quantity.Add(new ProductInCart(p, q));
            await ProxyMarketContext.Instance.saveChanges();
            return "product added to shopping basket";
        }

        public virtual bool RemoveProduct(Product product)
        {
            if(_product_quantity.Where(p => p.product.Id.Equals(product.Id)).Any())
            {
                MarketDAL.Instance.removeProductFromCart(_product_quantity.Where(p => p.product.Id.Equals(product.Id)).Single());
                _product_quantity.RemoveWhere(p => p.product.Id.Equals(product.Id));
                return true;
            }
               
            return false;
        }

        public virtual void UpdateProduct(Product product, int quantity)
        {
            if (!_product_quantity.Where(p => p.product.Id.Equals(product.Id)).Any())
            {
                _product_quantity.Add(new ProductInCart(product, quantity));
            }
            else
            {
                _product_quantity.Where(p => p.product.Id.Equals(product.Id)).First().quantity = quantity;
            }
        }

        public virtual bool TryUpdateProduct(Product product, int quantity)
        {
            if (!_product_quantity.Where(p => p.product.Id.Equals(product.Id)).Any())
            {
                return false;
            }
            else
            {
                _product_quantity.Where(p => p.product.Id.Equals(product.Id)).First().quantity = quantity;
                return true;
            }
        }

        public virtual double CalcCost()
        {
            return _product_quantity.Aggregate(0.0, (total, next) => total + next.product.Price * next.quantity);
        }

        public virtual ICollection<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            foreach(ProductInCart p in _product_quantity)
            {
                products.Add(p.product);
            }
            return products;
        }

        public virtual int GetProductQuantity(Product product)
        {
            if (_product_quantity.Where(p => p.product.Id.Equals(product.Id)).Any())
            {
                return _product_quantity.Where(p => p.product.Id.Equals(product.Id)).First().quantity;
            }
            return 0;
        }

        public virtual HashSet<ProductInCart> GetDictionaryProductQuantity()
        {
            return _product_quantity;
        }

        public int CompareTo(object obj)
        {
            return obj.GetHashCode() - GetHashCode();
        }

        public virtual Store GetStore()
        {
            return store;
        }

        public ShoppingCart GetShoppingCart()
        {
            return this.ShoppingCart;
        }
    }
}
