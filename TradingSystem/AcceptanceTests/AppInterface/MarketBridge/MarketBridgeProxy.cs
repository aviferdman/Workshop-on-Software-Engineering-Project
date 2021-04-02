using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        private Dictionary<ShopInfo, ShopRefs> shops;

        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge)
        {
            shops = new Dictionary<ShopInfo, ShopRefs>();
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
                exists = shops.TryGetValue(shopInfo, out shop);
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
                    shops.Add(shopInfo, shop);
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

        public bool AddProductToUserCart(ProductId product)
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
    }
}
