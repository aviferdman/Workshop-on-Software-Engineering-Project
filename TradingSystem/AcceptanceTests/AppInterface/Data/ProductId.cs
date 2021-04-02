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

        public override bool Equals(object? obj) => obj is ProductId other && Equals(other);
        public bool Equals(ProductId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Product Id {Value}";
        }
    }
}
