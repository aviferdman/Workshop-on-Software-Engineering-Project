namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductInCart
    {
        public ProductId ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductInCart(ProductId productId, int quantity) : this()
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProductInCart other && Equals(other);
        }
        public bool Equals(ProductInCart other)
        {
            return ProductId == other.ProductId &&
                Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
