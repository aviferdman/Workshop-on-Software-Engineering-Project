﻿using System;

using TradingSystem.Business.Market.DiscountPackage;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class DiscountDataCompundDTO
    {
        public string? Creator { get; set; } = "";
        public string? DiscountRuleRelation { get; set; } = "";
        public Guid DiscountId1 { get; set; }
        public Guid DiscountId2 { get; set; }
        public bool Decide { get; set; }

        public static DiscountDataCompundDTO FromDiscountRelation(DiscountsRelation discountsRelation)
        {
            return new DiscountDataCompundDTO
            {
                Creator = discountsRelation.Username,
                DiscountRuleRelation = discountsRelation.DiscountRuleRelation.ToString(),
                DiscountId1 = discountsRelation.DiscountId1,
                DiscountId2 = discountsRelation.DiscountId2,
                Decide = discountsRelation.Decide,
            };
        }
    }
}
