using System;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductId
    {
        public ProductId(ShopId shopId, string productName)
        {
            ShopId = shopId;
            ProductName = productName;
        }

        public ShopId ShopId { get; }
        public string ProductName { get; }

        public bool IsValid()
        {
            return !Guid.Empty.Equals(ShopId.Value)
                && !string.IsNullOrWhiteSpace(ProductName);
        }

        public override bool Equals(object? obj) => obj is ProductId other && Equals(other);
        public bool Equals(ProductId other) => other.ShopId == ShopId && other.ProductName.Equals(ProductName);
        public override int GetHashCode() => HashCode.Combine(ShopId, ProductName);
        public override string ToString()
        {
            return $"Product Id {{{ShopId}, {ProductName}}}";
        }

        public static bool operator ==(ProductId x1, ProductId x2)
        {
            return x1.Equals(x2);
        }
        public static bool operator !=(ProductId x1, ProductId x2)
        {
            return !(x1 == x2);
        }
    }
}
