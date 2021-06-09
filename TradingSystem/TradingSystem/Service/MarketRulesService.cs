using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StorePackage.PolicyPackage;
using TradingSystem.DAL;

namespace TradingSystem.Service
{
    public class MarketRulesService
    {
        private static readonly Lazy<MarketRulesService> instanceLazy = new Lazy<MarketRulesService>(() => new MarketRulesService(), true);

        private readonly MarketRules marketRules;
        private readonly DiscountsManager discountsManager;
        private readonly PolicyManager policyManager;
        private IDictionary<string, ICollection<IRule>> user_rules;
        int counter;
        public static MarketRulesService Instance { get { return instanceLazy.Value; } }
        private MarketRulesService()
        {
            marketRules = MarketRules.Instance;
            discountsManager = new DiscountsManager();
            policyManager = new PolicyManager();
            user_rules = new Dictionary<string, ICollection<IRule>>();
            counter = MarketDAL.Instance.getRuleCounter().Result;
        }

        //Add New / Complex Discounts
        public async Task<Guid> AddSimpleDiscountAsync(string username, Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "CreateSimpleDiscountAsync", username, storeId, discountType, precent, category, productId);
            Guid discountId =  await marketRules.CreateSimpleDiscountAsync(username, storeId, discountType, precent, category, productId);
            var discountData = new DiscountData(discountId, username, storeId, discountType, RuleType.Simple, precent, category, productId, int.MaxValue, 0, default(DateTime), default(DateTime));
            await discountsManager.AddDiscount(discountData);
            return discountId;
        }
        public async Task<Guid> AddConditionalDiscountAsync(string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(),
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = default(DateTime), DateTime d2 = default(DateTime))
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "CreateConditionalDiscountAsync", username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            Guid discountId = await marketRules.CreateConditionalDiscountAsync(username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            var discountData = new DiscountData(discountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            await discountsManager.AddDiscount(discountData);
            return discountId;
        }

        public async Task<Guid> AddDiscountRuleAsync(string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId1, Guid discountId2, bool decide = false)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "GenerateConditionalDiscountsAsync", username, discountRuleRelation, storeId, discountId1, discountId2, decide);
            Guid discountId = await marketRules.GenerateConditionalDiscountsAsync(username, discountRuleRelation, storeId, discountId1, discountId2, decide);
            var discountRelation = new DiscountsRelation(username, discountRuleRelation, storeId, discountId1, discountId2, decide);
            await discountsManager.AddRelation(discountRelation);
            return discountId;
        }

        //Update Simple / Complex Discounts
        public async Task<Result<Guid>> UpdateSimpleDiscountAsync(Guid existingDiscountId, string username, Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, existingDiscountId, "UpdateSimpleDiscountAsync", username, storeId, discountType, precent, category, productId);
            if (discountsManager.IsComplexed(existingDiscountId))
            {
                return new Result<Guid>(new Guid(), true, "Impossible to update a discount which other discounts are complexed on.");
            }
            var res = await marketRules.UpdateSimpleDiscountAsync(existingDiscountId, username, storeId, discountType, precent, category, productId);
            Guid discountId = res.Ret;
            var discountData = new DiscountData(discountId, username, storeId, discountType, RuleType.Simple, precent, category, productId, int.MaxValue, 0, default(DateTime), default(DateTime));
            await discountsManager.RemoveDiscount(discountId);
            await discountsManager.AddDiscount(discountData);
            return res;
        }
        public async Task<Result<Guid>> UpdateConditionalDiscountAsync(Guid existingDiscountId, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(),
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = default(DateTime), DateTime d2 = default(DateTime))
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, existingDiscountId, "UpdateConditionalDiscountAsync", username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            if (discountsManager.IsComplexed(existingDiscountId))
            {
                return new Result<Guid>(new Guid(), true, "Impossible to update a discount which other discounts are complexed on.");
            }
            var res = await marketRules.UpdateConditionalDiscountAsync(existingDiscountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            Guid discountId = res.Ret;
            var discountData = new DiscountData(discountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            await discountsManager.RemoveDiscount(discountId);
            await discountsManager.AddDiscount(discountData);
            return res;
        }

        public async Task<Result<Guid>> RemoveDiscountAsync(string username, Guid storeId, Guid discountId)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "RemoveDiscountAsync", username, storeId, discountId);
            if (discountsManager.IsComplexed(discountId))
            {
                return new Result<Guid>(new Guid(), true, "Impossible to remove a discount which other discounts are complexed on.");
            }
            await marketRules.RemoveDiscountAsync(username, storeId, discountId);
            await discountsManager.RemoveDiscount(discountId);
            return new Result<Guid>(discountId, false, "");
        }

        public async Task<ICollection <DiscountData>> GetAllDiscounts(Guid storeId)
        {
            int serialN = 1;
            var discounts = await this.discountsManager.GetAllDiscounts(storeId);
            foreach (var d in discounts)
            {
                d.SerialNumber = serialN++;
            }
            return discounts;
        }

        public async Task<ICollection<DiscountsRelation>> GetAllDiscountsRelations(Guid storeId)
        {
            int serialN = 1;
            var realations = await this.discountsManager.GetAllDiscountsRelations(storeId);
            foreach (var r in realations)
            {
                r.SerialNumber = serialN++;
            }
            return realations;
        }

        //Add New / Complex Policy
        public async Task AddPolicyRule(string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(),
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = default(DateTime), DateTime d2 = default(DateTime))
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "AddPolicyRule", username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            await marketRules.AddPolicyRule(username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            var policyData = new PolicyData(username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            await this.policyManager.AddPolicy(policyData);
        }
        public async Task RemovePolicyRule(string username, Guid storeId)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "RemovePolicyRuleAsync", username, storeId);
            await marketRules.RemovePolicyRuleAsync(username, storeId);
            await policyManager.RemovePolicy(storeId);
        }

        public async Task<List<PolicyData>> GetAllPolicies(Guid storeId)
        {
            return await this.policyManager.GetAllPolicies(storeId);
        } 

        private Guid CreateRule(string username, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = default(DateTime), DateTime d2 = default(DateTime))
        {
            var r = marketRules.CreateRule(ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            if (user_rules.Keys.Contains(username))
            {
                user_rules.Add(username, new HashSet<IRule>());
            }
            user_rules[username].Add(r);
            return r.GetId();
        }

    }
}
