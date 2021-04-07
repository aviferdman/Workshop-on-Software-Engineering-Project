using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductId
    {
        public ProductId(int id)
        {
            Value = id;
        }

        public int Value { get; }

        public bool IsValid()
        {
            return Value > 0;
        }

        public override bool Equals(object? obj) => obj is ProductId other && Equals(other);
        public bool Equals(ProductId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Product Id {Value}";
        }

        public static implicit operator ProductId(int id)
        {
            return new ProductId(id);
        }
        public static implicit operator int(ProductId productId)
        {
            return productId.Value;
        }
    }
}
