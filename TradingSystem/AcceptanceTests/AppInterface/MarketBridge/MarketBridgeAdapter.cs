using System;
using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeAdapter : IMarketBridge
    {
        private readonly SystemContext systemContext;
        private readonly MarketStoreGeneralService marketStoreGeneralService;

        private MarketBridgeAdapter(SystemContext systemContext, MarketStoreGeneralService marketStoreGeneralService)
        {
            this.systemContext = systemContext;
            this.marketStoreGeneralService = marketStoreGeneralService;
        }

        public static MarketBridgeAdapter New(SystemContext systemContext)
        {
            return new MarketBridgeAdapter(systemContext, MarketStoreGeneralService.Instance);
        }

        public ProductSearchResults? SearchProducts(ProductSearchCreteria creteria)
        {
            throw new NotImplementedException();
        }

        public ShopId? OpenShop(ShopInfo shopInfo)
        {
            StoreData storeData = marketStoreGeneralService.CreateStore
            (
                shopInfo.Name,
                systemContext.TokenUsername,
                shopInfo.BankAccount.AccountNumber,
                shopInfo.BankAccount.Branch,
                shopInfo.Address.State,
                shopInfo.Address.City,
                shopInfo.Address.Street,
                shopInfo.Address.ApartmentNum
            );
            return new ShopId(storeData.Id);
        }

        public ShopId? AssureOpenShop(ShopInfo shopInfo)
        {
            throw new InvalidOperationException($"Should not call this method, use {nameof(OpenShop)} method instead.");
        }

        public ShopInfo? GetShopDetails(ShopId shopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductIdentifiable>? GetShopProducts(ShopId shopId)
        {
            throw new NotImplementedException();
        }

        public ProductId? AddProductToShop(ShopId shopId, ProductInfo productInfo)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromShop(ShopId shopId, ProductId productId)
        {
            throw new NotImplementedException();
        }

        public bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductInCart>? GetShoppingCartItems()
        {
            throw new NotImplementedException();
        }

        public bool AddProductToUserCart(ProductInCart product)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromUserCart(ProductId productId)
        {
            throw new NotImplementedException();
        }

        public bool EditProductInUserCart(ProductId productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public bool EditUserCart(ISet<ProductInCart> productsAdd, ISet<ProductId> productsRemove, ISet<ProductInCart> productsEdit)
        {
            throw new NotImplementedException();
        }
    }
}
