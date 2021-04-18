using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeAdapter : BridgeAdapterBase, IMarketBridge
    {
        private readonly MarketStoreGeneralService marketStoreGeneralService;
        private readonly MarketProductsService marketProductsService;
        private readonly MarketShoppingCartService marketShoppingCartService;

        private MarketBridgeAdapter
        (
            SystemContext systemContext,
            MarketStoreGeneralService marketStoreGeneralService,
            MarketProductsService marketProductsService,
            MarketShoppingCartService marketShoppingCartService
        )
            : base(systemContext)
        {
            this.marketStoreGeneralService = marketStoreGeneralService;
            this.marketProductsService = marketProductsService;
            this.marketShoppingCartService = marketShoppingCartService;
        }

        public static MarketBridgeAdapter New(SystemContext systemContext)
        {
            return new MarketBridgeAdapter
            (
                systemContext,
                MarketStoreGeneralService.Instance,
                MarketProductsService.Instance,
                MarketShoppingCartService.Instance
            );
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
                Username,
                shopInfo.BankAccount.AccountNumber,
                shopInfo.BankAccount.Branch,
                shopInfo.Address.State,
                shopInfo.Address.City,
                shopInfo.Address.Street,
                shopInfo.Address.ApartmentNum
            );
            return storeData == null ? (ShopId?)null : new ShopId(storeData.Id);
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
            Result<Product> result = marketProductsService.AddProduct
            (
                new ProductData
                (
                    name: productInfo.Name,
                    quantity: productInfo.Quantity,
                    weight: productInfo.Weight,
                    price: productInfo.Price,
                    category: productInfo.Category
                ),
                shopId,
                Username
            );
            return result.IsErr ? (ProductId?)null : new ProductId(result.Ret.Id);
        }

        public bool RemoveProductFromShop(ShopId shopId, ProductId productId)
        {
            string result = marketProductsService.RemoveProduct(productId, shopId, Username);
            return result == "Product removed";
        }

        public bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails)
        {
            string result = marketProductsService.EditProduct
            (
                productId,
                new ProductData
                (
                    name: newProductDetails.Name,
                    quantity: newProductDetails.Quantity,
                    weight: newProductDetails.Weight,
                    price: newProductDetails.Price,
                    category: newProductDetails.Category
                ),
                shopId,
                Username
            );
            return result == "Product edited";
        }

        public IEnumerable<ProductInCart>? GetShoppingCartItems()
        {
            IDictionary<Guid, Dictionary<ProductData, int>> shoppingCartData = marketShoppingCartService.ViewShoppingCart(Username);
            return shoppingCartData
                .SelectMany
                (
                    shoppingBasket => shoppingBasket.Value
                    .Select(prodcutInCart => new ProductInCart(prodcutInCart.Key.pid, prodcutInCart.Value))
                );
        }

        public bool AddProductToUserCart(ProductInCart product)
        {
            return EditUserCart
            (
                new ProductInCart[] { product },
                Enumerable.Empty<ProductId>(),
                Enumerable.Empty<ProductInCart>()
            );
        }

        public bool RemoveProductFromUserCart(ProductId productId)
        {
            return EditUserCart
            (
                Enumerable.Empty<ProductInCart>(),
                new ProductId[] { productId },
                Enumerable.Empty<ProductInCart>()
            );
        }

        public bool EditProductInUserCart(ProductInCart product)
        {
            return EditUserCart
            (
                Enumerable.Empty<ProductInCart>(),
                Enumerable.Empty<ProductId>(),
                new ProductInCart[] { product }
            );
        }

        public bool EditUserCart(IEnumerable<ProductInCart> productsAdd, IEnumerable<ProductId> productsRemove, IEnumerable<ProductInCart> productsEdit)
        {
            Result<Dictionary<Guid, Dictionary<ProductData, int>>>? result = marketShoppingCartService.EditShoppingCart
            (
                Username,
                productsRemove.Select(x => x.Value).ToList(),
                ProductInCart.ToDictionary(productsAdd),
                ProductInCart.ToDictionary(productsEdit)
            );
            return result != null && !result.IsErr;
        }
    }
}
