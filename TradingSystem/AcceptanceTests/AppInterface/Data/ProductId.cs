using System;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductId
    {
        public ProductId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public bool IsValid()
        {
            return !Guid.Empty.Equals(Value);
        }

        public override bool Equals(object? obj) => obj is ProductId other && Equals(other);
        public bool Equals(ProductId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Product Id {Value}";
        }

        public static bool operator ==(ProductId x1, ProductId x2)
        {
            return x1.Equals(x2);
        }
        public static bool operator !=(ProductId x1, ProductId x2)
        {
            return !(x1 == x2);
        }

        public static implicit operator ProductId(Guid id)
        {
            return new ProductId(id);
        }
        public static implicit operator Guid(ProductId productId)
        {
            return productId.Value;
        }
    }
}
