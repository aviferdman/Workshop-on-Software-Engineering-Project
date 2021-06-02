using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market.DiscountPackage
{
    public class DiscountsManager
    {
        private HashSet<DiscountData> discountsData;
        private HashSet<DiscountsRelation> discountsRelations;

        public HashSet<DiscountData> DiscountsData { get => discountsData; set => discountsData = value; }
        public HashSet<DiscountsRelation> DiscountsRelations { get => discountsRelations; set => discountsRelations = value; }

        public DiscountsManager()
        {
            this.DiscountsData = new HashSet<DiscountData>();
            this.DiscountsRelations = new HashSet<DiscountsRelation>();
        }

        public async Task AddDiscount(DiscountData d)
        {
            this.DiscountsData.Add(d);
        }

        public async Task RemoveDiscount(Guid discountId)
        {
            this.DiscountsData.RemoveWhere(d => d.DiscountId.Equals(discountId));
        }

        public async Task AddRelation(DiscountsRelation discountsRelation)
        {
            this.DiscountsRelations.Add(discountsRelation);
        }

        public async Task RemoveRelation(Guid discountId)
        {
            this.DiscountsRelations.RemoveWhere(d => d.DiscountId1.Equals(discountId) || d.DiscountId2.Equals(discountId));
        }

        public async Task<ICollection<DiscountData>> GetAllDiscounts(Guid storeId)
        {
            return this.DiscountsData.Where(d => d.StoreId.Equals(storeId)).ToList();
        }

        public async Task<ICollection<DiscountsRelation>> GetAllDiscountsRelations(Guid storeId)
        {
            return this.DiscountsRelations.Where(d => d.StoreId.Equals(storeId)).ToList();
        }
    }
}
