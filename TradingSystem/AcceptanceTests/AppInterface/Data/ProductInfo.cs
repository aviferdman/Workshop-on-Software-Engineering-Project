using TradingSystem.Business.Market;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductInfo
    {
        public ProductInfo(string name, int quantity, double price, string category, double weight)
        {
            Name = name;
            Price = price;
            Category = category;
            Weight = weight;
            Quantity = quantity;
        }

        public static ProductData ToProductData(ProductInfo productInfo)
        {
            return new ProductData
            (
                name: productInfo.Name,
                quantity: productInfo.Quantity,
                weight: productInfo.Weight,
                price: productInfo.Price,
                category: productInfo.Category
            );
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public double Weight { get; set; }
    }
}
