using System;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ShopId
    {
        public ShopId(Guid id)
        {
            Value = id;
        }

        public Guid Value { get; }

        public override bool Equals(object? obj) => obj is ShopId other && Equals(other);
        public bool Equals(ShopId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Shop Id {Value}";
        }

        public static implicit operator ShopId(Guid id)
        {
            return new ShopId(id);
        }
        public static implicit operator Guid(ShopId shopId)
        {
            return shopId.Value;
        }
    }
}
