using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public struct ProductCartEditInfo
    {
        public ProductCartEditInfo(ProductIdentifiable productOriginal, int newQuantity)
        {
            ProductOriginal = productOriginal;
            NewQuantity = newQuantity;
        }

        public ProductIdentifiable ProductOriginal { get; }
        public int NewQuantity { get; }
    }
}
