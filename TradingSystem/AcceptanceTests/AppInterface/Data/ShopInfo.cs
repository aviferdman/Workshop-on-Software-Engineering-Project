namespace AcceptanceTests.AppInterface.Data
{
    public class ShopInfo
    {
        public ShopInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object? obj) => obj is ShopInfo other && Equals(other);
        public bool Equals(ShopInfo other) => Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override string? ToString() => Name;
    }
}
