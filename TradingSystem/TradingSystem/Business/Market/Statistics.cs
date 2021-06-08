using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Statistics
    {
        public DateTime date { get; set; }
        public int guestsNum { get; set; }
        public int membersNum { get; set; }
        public int ownersNum { get; set; }
        public int managersNum { get; set; }
        public int adminNum { get; set; }

        public Statistics()
        {
            date = DateTime.Now.Date;
            guestsNum = 0;
            membersNum = 0;
            ownersNum = 0;
            managersNum = 0;
            adminNum = 0;
        }
    }
}
