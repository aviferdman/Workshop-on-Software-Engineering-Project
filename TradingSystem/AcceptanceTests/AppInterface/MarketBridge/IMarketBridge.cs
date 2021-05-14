using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptanceTests.AppInterface.Data;

using Moq;

using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        ProductSearchResults? SearchProducts(ProductSearchCreteria creteria);

        /// <summary>
        /// Tries to open the shop with the specified info.
        /// </summary>
        /// <returns>The opened shop id, null if failed.</returns>
        ShopId? OpenShop(ShopInfo shopInfo);

        /// <summary>
        /// Makes sure the shop with the specified shop is open and returns the shop id.
        /// Might send a request to create a new shop.
        /// Since shops cannot be deleted, we take similar measures to
        /// <seealso cref="UserBridge.IUserBridge.AssureSignUp(UserInfo)"/>
        /// </summary>
        /// <returns>The opened shop id, null if failed.</returns>
        ShopId? AssureOpenShop(ShopInfo shopInfo);

        ShopInfo? GetShopDetails(ShopId shopId);
        IEnumerable<ProductIdentifiable>? GetShopProducts(ShopId shopId);

        /// <summary>
        /// Adds the product to the shop and returns the product id.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productInfo"></param>
        /// <returns>The product id, null if failed.</returns>
        ProductId? AddProductToShop(ShopId shopId, ProductInfo productInfo);
        bool RemoveProductFromShop(ShopId shopId, ProductId productId);
        bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails);

        Task<bool> PurchaseShoppingCart(PurchaseInfo purchaseInfo);
        IEnumerable<ProductInCart>? GetShoppingCartItems();

        bool AddProductToUserCart(ProductInCart product);
        bool EditProductInUserCart(ProductInCart product);
        bool RemoveProductFromUserCart(ProductId productId);
        bool EditUserCart(IEnumerable<ProductInCart> productsAdd, IEnumerable<ProductId> productsRemove, IEnumerable<ProductInCart> productsEdit);
        bool MakeOwner(string assignee, Guid storeID, string assigner);
        bool MakeManager(string assignee, Guid storeID, string assigner);
        bool RemoveOwner(String ownerName, Guid storeID, String assignerName);
        bool DefineManagerPermissions(string manager, Guid storeID, string assigner, List<Permission> permissions);
        bool RemoveManager(String managerName, Guid storeID, String assignerName);
        void tearDown();
        PurchaseHistory? GetUserPurchaseHistory();

        void SetExternalTransactionMocks(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem);
        void DisableExternalTransactionMocks();
    }
}
