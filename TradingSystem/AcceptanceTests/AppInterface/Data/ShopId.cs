namespace AcceptanceTests.AppInterface.Data
{
    public struct ShopId
    {
        public ShopId(int id)
        {
            Value = id;
        }

        public int Value { get; }

        public override bool Equals(object? obj) => obj is ShopId other && Equals(other);
        public bool Equals(ShopId other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString()
        {
            return $"Shop Id {Value}";
        }

        public static implicit operator ShopId(int id)
        {
            return new ShopId(id);
        }
        public static implicit operator int(ShopId shopId)
        {
            return shopId.Value;
        }
    }
}
