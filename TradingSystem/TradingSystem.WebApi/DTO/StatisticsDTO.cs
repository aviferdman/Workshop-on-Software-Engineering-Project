using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO
{
    public class StatisticsDTO
    {
        public int Guests { get; set; }
        public int PureMembers { get; set; }
        public int PureManagers { get; set; }
        public int Owners { get; set; }
        public int Admins { get; set; }

        public static StatisticsDTO FromStatistics(Statistics statistics)
        {
            return new StatisticsDTO
            {
                Guests = statistics.guestsNum,
                PureMembers = statistics.membersNum,
                PureManagers = statistics.managersNum,
                Owners = statistics.ownersNum,
                Admins = statistics.adminNum,
            };
        }
    }
}
