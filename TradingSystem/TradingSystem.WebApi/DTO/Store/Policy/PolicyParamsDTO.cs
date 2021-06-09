using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Policy
{
    public class PolicyParamsDTO
    {
        [Required]
        public string Username { get; set; } = "";
        public Guid StoreId { get; set; }

        [Required]
        public string RuleRelation { get; set; } = "";
        public string? RuleType { get; set; }
        public string RuleContext { get; set; } = "";

        public string? Category { get; set; }
        public Guid? ProductId { get; set; }


        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
