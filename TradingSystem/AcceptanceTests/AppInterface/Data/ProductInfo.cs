namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductInfo
    {
        public ProductInfo(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public string Name { get; }
        public int Price { get; }
        public int Quantity { get; }
    }
}
