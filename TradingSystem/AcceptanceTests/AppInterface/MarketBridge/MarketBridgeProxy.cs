using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

using Moq;

using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        private readonly ConcurrentDictionary<string, ShopRefs> shops;

        public MarketBridgeProxy() : this(null) { }
        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge)
        {
            shops = new ConcurrentDictionary<string, ShopRefs>();
        }

        public override IMarketBridge Bridge => this;

        public ProductSearchResults? SearchProducts(ProductSearchCreteria creteria)
        {
            return RealBridge?.SearchProducts(creteria);
        }

        public ShopId? OpenShop(ShopInfo shopInfo)
        {
            return RealBridge?.OpenShop(shopInfo);
        }

        public ShopId? AssureOpenShop(ShopInfo shopInfo)
        {
            if (RealBridge == null)
            {
                return null;
            }

            ShopRefs? shopRef;
            ShopId? shopId;
            try
            {
                shopRef = shops.GetOrAdd
                (
                    shopInfo.Name,
                    shopName =>
                    {
                        ShopId? shopId = RealBridge.OpenShop(shopInfo);
                        if (shopId == null)
                        {
                            throw new OperationFailedException();
                        }

                        return new ShopRefs(shopId.Value, SystemContext!.LoggedInUser!);
                    }
                );

                shopId = shopRef.Id;
            }
            catch (OperationFailedException)
            {
                shopId = null;
            }

            return shopId;
        }

        public ShopInfo? GetShopDetails(ShopId shopId)
        {
            try
            {
                return RealBridge?.GetShopDetails(shopId);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }

        public IEnumerable<ProductIdentifiable>? GetShopProducts(ShopId shopId)
        {
            return RealBridge?.GetShopProducts(shopId);
        }

        public ProductId? AddProductToShop(ShopId shop, ProductInfo productInfo)
        {
            return RealBridge?.AddProductToShop(shop, productInfo);
        }

        public bool RemoveProductFromShop(ShopId shop, ProductId product)
        {
            return RealBridge != null && RealBridge.RemoveProductFromShop(shop, product);
        }

        public bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails)
        {
            return RealBridge != null && RealBridge.EditProductInShop(shopId, productId, newProductDetails);
        }

        public IEnumerable<ProductInCart>? GetShoppingCartItems()
        {
            return RealBridge?.GetShoppingCartItems();
        }

        public bool AddProductToUserCart(ProductInCart product)
        {
            return RealBridge != null && RealBridge.AddProductToUserCart(product);
        }

        public bool RemoveProductFromUserCart(ProductId productId)
        {
            return RealBridge != null && RealBridge.RemoveProductFromUserCart(productId);
        }

        public bool EditProductInUserCart(ProductInCart product)
        {
            return RealBridge != null && RealBridge.EditProductInUserCart(product);
        }

        public bool EditUserCart(IEnumerable<ProductInCart> productsAdd, IEnumerable<ProductId> productsRemove, IEnumerable<ProductInCart> productsEdit)
        {
            return RealBridge != null && RealBridge.EditUserCart(productsAdd, productsRemove, productsEdit);
        }

        public bool PurchaseShoppingCart(PurchaseInfo purchaseInfo)
        {
            return RealBridge != null && RealBridge.PurchaseShoppingCart(purchaseInfo);
        }

        public void SetExternalTransactionMocks(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem)
        {
            if (RealBridge != null)
            {
                RealBridge.SetExternalTransactionMocks(deliverySystem, paymentSystem);
            }
        }

        public void DisableExternalTransactionMocks()
        {
            if (RealBridge != null)
            {
                RealBridge.DisableExternalTransactionMocks();
            }
        }

        public PurchaseHistory? GetUserPurchaseHistory()
        {
            return RealBridge?.GetUserPurchaseHistory();
        }
    }
}
