using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    /// <summary>
    /// Acceptance test for
    /// Use case 25: edit product in shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/20
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_EditShopProductInShop : ShopManagementTestBase
    {
        private static object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                User_ShopOwner1,
                new ShopInfo(SHOP_NAME),
                new ProductEditInfo[]
                {
                    new ProductEditInfo(
                        new ProductInfo("WiiU", 1000, 80),
                        new ProductInfo("WiiU", 1100, 90)
                    ),
                    new ProductEditInfo(
                        new ProductInfo("garbage can", 20, 100),
                        new ProductInfo("garbage can not haHAA", 15, 80)
                    ),
                },
            },
        };

        private UseCase_AddProductToShop useCase_addProduct;

        public UseCase_EditShopProductInShop(SystemContext systemContext, UserInfo shopOwnerUser, ShopInfo shopInfo, IEnumerable<ProductEditInfo> productInfos) :
            base(systemContext, shopOwnerUser)
        {
            if (productInfos is null)
            {
                throw new ArgumentNullException(nameof(productInfos));
            }
            if (!productInfos.Any())
            {
                throw new ArgumentException("Must contain least have 1 product", nameof(productInfos));
            }

            ShopInfo = shopInfo;
            ProductEditInfos = productInfos;
            AddedProductIds = new List<ProductId>();
        }

        public ShopId ShopId => useCase_addProduct.ShopId;

        public ShopInfo ShopInfo { get; }
        public IEnumerable<ProductEditInfo> ProductEditInfos { get; }
        private IList<ProductId> AddedProductIds { get; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_addProduct = new UseCase_AddProductToShop(SystemContext.Instance, UserInfo, ShopInfo.Name);
            useCase_addProduct.Setup();
            foreach (ProductEditInfo productEditInfo in ProductEditInfos)
            {
                ProductId productId = useCase_addProduct.Success_Normal(productEditInfo.ProductInfoOriginal);
                AddedProductIds.Add(productId);
            }
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_addProduct.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            IEnumerable<(ProductInfo ProductEditInfo, ProductId ProductId)> productInfoEdits = ProductEditInfos
                .Select(x => x.ProductInfoEdit)
                .Zip(AddedProductIds);
            foreach ((ProductInfo ProductEditInfo, ProductId ProductId) item in productInfoEdits)
            {
                Assert.IsTrue(Bridge.EditProductInShop(ShopId, item.ProductId, item.ProductEditInfo));
            }
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, AddedProductIds.First(), ProductEditInfos.First().ProductInfoEdit));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, new ProductId(int.MaxValue - 1), ProductEditInfos.First().ProductInfoEdit));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsNull(Bridge.EditProductInShop(ShopId, AddedProductIds.First(), new ProductInfo("ineditcucumber", -7, 23)));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsNull(Bridge.EditProductInShop(ShopId, AddedProductIds.First(), new ProductInfo("", 14, 23)));
        }
    }
}
