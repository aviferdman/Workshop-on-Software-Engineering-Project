using System;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ShopId
    {
        public ShopId(Guid id, string shopName)
        {
            Value = id;
            ShopName = shopName;
        }

        public Guid Value { get; }
        public string ShopName { get; }

        public override bool Equals(object? obj) => obj is ShopId other && Equals(other);
        public bool Equals(ShopId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Shop Id {Value}";
        }

        public static bool operator ==(ShopId x1, ShopId x2)
        {
            return x1.Equals(x2);
        }
        public static bool operator !=(ShopId x1, ShopId x2)
        {
            return !(x1 == x2);
        }

        //public static implicit operator ShopId(Guid id)
        //{
        //    return new ShopId(id);
        //}
        public static implicit operator Guid(ShopId shopId)
        {
            return shopId.Value;
        }
    }
}
