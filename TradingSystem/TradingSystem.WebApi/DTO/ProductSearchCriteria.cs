using System.ComponentModel;

namespace TradingSystem.WebApi.DTO
{
    public class ProductSearchCreteria
    {
        public ProductSearchCreteria()
        {
            Keywords = null;
            Category = null;
            PriceRange_Low = -1;
            PriceRange_High = -1;
            ProductRating = -1;
        }

        public string? Keywords { get; set; }
        public string? Category { get; set; }
        [DefaultValue(-1)]
        public int PriceRange_Low { get; set; }
        [DefaultValue(-1)]
        public int PriceRange_High { get; set; }
        public int ProductRating { get; set; }
    }
}
