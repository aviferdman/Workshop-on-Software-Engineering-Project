using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        private readonly Dictionary<string, ShopRefs> shops;

        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge)
        {
            shops = new Dictionary<string, ShopRefs>();
        }

        public override IMarketBridge Bridge => this;

        public ShopId? AssureOpenShop(ShopInfo shopInfo)
        {
            if (RealBridge == null)
            {
                return null;
            }

            ShopRefs? shop;
            bool exists;
            lock (shops)
            {
                exists = shops.TryGetValue(shopInfo.Name, out shop);
            }
            if (exists)
            {
                if (!shop!.Owner.Equals(SystemContext!.LoggedInUser))
                {
                    throw new ShopOwnerMismatchException(shopInfo);
                }

                return shop.Id;
            }

            ShopId? shopId = RealBridge.AssureOpenShop(shopInfo);
            if (shop != null)
            {
                lock (shops)
                {
                    shops.Add(shopInfo.Name, shop);
                }
            }
            return shopId;
        }

        public ProductId? AddProductToShop(ShopId shop, ProductInfo productInfo)
        {
            return RealBridge?.AddProductToShop(shop, productInfo);
        }

        public bool RemoveProductFromShop(ShopId shop, ProductId product)
        {
            return RealBridge != null && RealBridge.RemoveProductFromShop(shop, product);
        }

        public ProductSearchResults? SearchProducts(ProductSearchCreteria creteria)
        {
            return RealBridge?.SearchProducts(creteria);
        }

        public bool AddProductToUserCart(ProductInCart product)
        {
            return RealBridge != null && RealBridge.AddProductToUserCart(product);
        }

        public bool RemoveProductFromUserCart(ProductId productId)
        {
            return RealBridge != null && RealBridge.RemoveProductFromUserCart(productId);
        }

        public IEnumerable<ProductId>? GetShoppingCartItems()
        {
            return RealBridge?.GetShoppingCartItems();
        }

        public ShopInfo? GetShopDetails(ShopId shopId)
        {
            return RealBridge?.GetShopDetails(shopId);
        }

        public IEnumerable<ProductId>? GetShopProducts(ShopId shopId)
        {
            return RealBridge?.GetShopProducts(shopId);
        }

        public bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails)
        {
            return RealBridge != null && RealBridge.EditProductInShop(shopId, productId, newProductDetails);
        }

        public bool EditProductInUserCart(ProductId productId, int quantity)
        {
            return RealBridge != null && RealBridge.EditProductInUserCart(productId, quantity);
        }

        public bool EditUserCart(ISet<ProductInCart> productsAdd, ISet<ProductId> productsRemove, ISet<ProductInCart> productsEdit)
        {
            return RealBridge != null && RealBridge.EditUserCart(productsAdd, productsRemove, productsEdit);
        }
    }
}
