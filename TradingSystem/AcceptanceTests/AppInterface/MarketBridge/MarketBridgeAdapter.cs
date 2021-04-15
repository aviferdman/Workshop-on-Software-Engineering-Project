﻿using System;
using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeAdapter : IMarketBridge
    {
        private readonly SystemContext systemContext;
        private readonly MarketStoreGeneralService marketStoreGeneralService;
        private readonly MarketProductsService marketProductsService;

        private MarketBridgeAdapter
        (
            SystemContext systemContext,
            MarketStoreGeneralService marketStoreGeneralService,
            MarketProductsService marketProductsService
        )
        {
            this.systemContext = systemContext;
            this.marketStoreGeneralService = marketStoreGeneralService;
            this.marketProductsService = marketProductsService;
        }

        public static MarketBridgeAdapter New(SystemContext systemContext)
        {
            return new MarketBridgeAdapter
            (
                systemContext,
                MarketStoreGeneralService.Instance,
                MarketProductsService.Instance
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
                systemContext.TokenUsername,
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
            string result = marketProductsService.AddProduct
            (
                new ProductData(new Product
                (
                    name: productInfo.Name,
                    quantity: productInfo.Quantity,
                    weight: productInfo.Weight,
                    price: productInfo.Price
                ))
                {
                    category = productInfo.Category,
                },
                shopId,
                systemContext.TokenUsername
            );
            bool success = result == "Product added";
            return success ? new ProductId(shopId, productInfo.Name) : (ProductId?)null;
        }

        public bool RemoveProductFromShop(ShopId shopId, ProductId productId)
        {
            string result = marketProductsService.RemoveProduct
            (
                productId.ProductName,
                shopId.Value, systemContext.TokenUsername
            );
            return result == "Product removed";
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
