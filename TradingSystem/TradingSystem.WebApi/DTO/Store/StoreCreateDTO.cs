namespace TradingSystem.WebApi.DTO
{
    public class StoreCreateDTO
    {
        public string? Username { get; set; }
        public string? StoreName { get; set; }
        public Address? Address { get; set; }
        public CreditCard? CreditCard { get; set; }
    }
}
