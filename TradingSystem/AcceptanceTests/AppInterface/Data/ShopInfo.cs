namespace AcceptanceTests.AppInterface.Data
{
    public class ShopInfo
    {
        public ShopInfo(string name, CreditCard creditCard, Address address)
        {
            if (creditCard is null)
            {
                throw new System.ArgumentNullException(nameof(creditCard));
            }

            Name = name;
            CreditCard = creditCard;
            Address = address;
        }

        public string Name { get; }
        public CreditCard CreditCard { get; }
        public Address Address { get; }

        public override bool Equals(object? obj) => obj is ShopInfo other && Equals(other);
        public bool Equals(ShopInfo other) => Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override string? ToString() => Name;
    }
}
