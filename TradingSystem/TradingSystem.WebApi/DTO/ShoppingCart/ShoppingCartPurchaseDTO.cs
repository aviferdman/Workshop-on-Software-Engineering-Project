namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartPurchaseDTO
    {
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public Address? Address { get; set; }
        public CreditCard? CreditCard { get; set; }
    }
}
