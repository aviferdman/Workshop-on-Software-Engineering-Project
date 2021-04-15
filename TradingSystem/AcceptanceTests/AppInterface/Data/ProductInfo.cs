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

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public double Weight { get; set;
        }
    }
}
