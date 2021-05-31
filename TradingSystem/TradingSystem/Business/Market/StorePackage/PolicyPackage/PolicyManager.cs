using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market.StorePackage.PolicyPackage
{
    public class PolicyManager
    {
        private HashSet<PolicyData> policiesData;
        private int time;

        public PolicyManager()
        {
            policiesData = new HashSet<PolicyData>();
            time = 0;
        }
        public HashSet<PolicyData> PoliciesData { get => policiesData; set => policiesData = value; }

        public async Task AddPolicy(PolicyData policyData)
        {
            policyData.Time = time;
            this.policiesData.Add(policyData);

            time++;
        }

        public async Task RemovePolicy(Guid storeId)
        {
            policiesData.RemoveWhere(p => p.StoreId.Equals(storeId));
        }

        public async Task<List<PolicyData>> GetAllPolicies(Guid storeId)
        {
            var policies = this.policiesData.Where(p => p.StoreId.Equals(storeId));
            return policies.OrderBy(p => p.Time).ToList();
        }
    }
}
