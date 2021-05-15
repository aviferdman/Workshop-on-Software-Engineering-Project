using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptanceTests.AppInterface.Data;

using Moq;

using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;
using TradingSystem.Service;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeAdapter : BridgeAdapterBase, IMarketBridge
    {
        private readonly MarketStoreGeneralService marketStoreGeneralService;
        private readonly MarketProductsService marketProductsService;
        private readonly MarketShoppingCartService marketShoppingCartService;
        private readonly MarketGeneralService marketGeneralService;
        private readonly MarketUserService marketUserService;
        private readonly MarketStorePermissionsManagementService marketStorePermissionsManagementService;

        private MarketBridgeAdapter
        (
            SystemContext systemContext,
            MarketStoreGeneralService marketStoreGeneralService,
            MarketProductsService marketProductsService,
            MarketShoppingCartService marketShoppingCartService,
            MarketGeneralService marketGeneralService,
            MarketUserService marketUserService,
            MarketStorePermissionsManagementService marketStorePermissionsManagementService
        )
            : base(systemContext)
        {
            this.marketStoreGeneralService = marketStoreGeneralService;
            this.marketProductsService = marketProductsService;
            this.marketShoppingCartService = marketShoppingCartService;
            this.marketGeneralService = marketGeneralService;
            this.marketUserService = marketUserService;
            this.marketStorePermissionsManagementService = marketStorePermissionsManagementService;
        }

        public static MarketBridgeAdapter New(SystemContext systemContext)
        {
            return new MarketBridgeAdapter
            (
                systemContext,
                MarketStoreGeneralService.Instance,
                MarketProductsService.Instance,
                MarketShoppingCartService.Instance,
                MarketGeneralService.Instance,
                MarketUserService.Instance,
                MarketStorePermissionsManagementService.Instance
            );
        }

        public ProductSearchResults? SearchProducts(ProductSearchCreteria creteria)
        {
            ICollection<ProductData>? results = marketProductsService.FindProducts
            (
                creteria.Keywords,
                (int)Math.Floor(creteria.PriceRange_Low),
                (int)Math.Ceiling(creteria.PriceRange_High),
                (int)creteria.ProductRating,
                creteria.Category
            );

            return new ProductSearchResults(results.Select(ProductIdentifiable.FromProductData), null);
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
            return storeData == null ? (ShopId?)null : new ShopId(storeData.Id, shopInfo.Name);
        }

        public ShopId? AssureOpenShop(ShopInfo shopInfo)
        {
            throw new InvalidOperationException($"Should not call this method, use {nameof(OpenShop)} method instead.");
        }

        public ShopInfo? GetShopDetails(ShopId shopId)
        {
            //ICollection<StoreData>? shopDetails = marketStoreGeneralService.FindStoresByName(shopId.ShopName);
            //if (shopDetails == null || shopDetails.Count == 0)
            //{
            //    return null;
            //}
            //if (shopDetails.Count > 1)
            //{
            //    throw new SharedDataMismatchException("There are multiple shops with the same name");
            //}

            //StoreData storeData = shopDetails.FirstOrDefault();
            //return new ShopInfo();
            throw new NotImplementedException("The service layer currently doesn't return any information about the shop.");
        }

        public IEnumerable<ProductIdentifiable>? GetShopProducts(ShopId shopId)
        {
            ICollection<ProductData>? products = marketProductsService.FindProductsByStores(shopId.ShopName);
            return products?.Select(ProductIdentifiable.FromProductData);
        }

        public ProductId? AddProductToShop(ShopId shopId, ProductInfo productInfo)
        {
            Result<Product> result = marketProductsService.AddProduct
            (
                ProductInfo.ToProductData(productInfo),
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
                ProductInfo.ToProductData(newProductDetails),
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

        public async Task<bool> PurchaseShoppingCart(PurchaseInfo purchaseInfo)
        {
            Result<bool> result = await marketShoppingCartService.PurchaseShoppingCart
            (
                SystemContext.TokenUsername,
                purchaseInfo.BankAccount.AccountNumber,
                purchaseInfo.BankAccount.Branch,
                purchaseInfo.PhoneNumber,
                purchaseInfo.Address.State,
                purchaseInfo.Address.City,
                purchaseInfo.Address.Street,
                purchaseInfo.Address.ApartmentNum
            );
            return !result.IsErr && result.Ret;
        }

        public bool MakeOwner(string assignee, Guid storeID, string assigner)
        {
            return marketStorePermissionsManagementService.MakeOwner(assignee, storeID, assigner).Equals("Success");
        }
        public bool MakeManager(string assignee, Guid storeID, string assigner)
        {
            return marketStorePermissionsManagementService.MakeManager(assignee, storeID, assigner).Equals("Success");
        }
        public bool RemoveOwner(String ownerName, Guid storeID, String assignerName)
        {
            return marketStorePermissionsManagementService.RemoveOwner(ownerName, storeID, assignerName).Equals("success");
        }
        public bool DefineManagerPermissions(string manager, Guid storeID, string assigner, List<Permission> permissions)
        {
            return marketStorePermissionsManagementService.DefineManagerPermissions(manager, storeID, assigner, permissions).Equals("Success");
        }
        public bool RemoveManager(String managerName, Guid storeID, String assignerName)
        {
            return marketStorePermissionsManagementService.RemoveManager(managerName, storeID, assignerName).Equals("success");
        }

        public void SetExternalTransactionMocks(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem)
        {
            marketGeneralService.ActivateDebugMode(deliverySystem, paymentSystem, true);
        }

        public void DisableExternalTransactionMocks()
        {
            marketGeneralService.ActivateDebugMode(null, null, false);
        }

        public PurchaseHistory? GetUserPurchaseHistory()
        {
            ICollection<HistoryData>? history = marketUserService.GetUserHistory(SystemContext.TokenUsername);
            if (history == null)
            {
                return null;
            }

            IEnumerable<PurchaseHistoryRecord> records = history.Select
            (
                x => new PurchaseHistoryRecord
                (
                    x.Products.ProductId_quantity.Select
                    (
                        (id_quantity) => new ProductInCart(id_quantity.Key, id_quantity.Value)
                    ),
                    x.Deliveries.PackageId,
                    x.Deliveries.Status,
                    x.Payments.PaymentId,
                    x.Payments.Status
                )
            );
            return new PurchaseHistory(records);
        }

        public void tearDown()
        {
            marketGeneralService.tearDown();
        }
    }
}
