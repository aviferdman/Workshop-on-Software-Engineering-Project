using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.Controllers
{
    public class SignUpInfo
    {
        public string guestusername { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string _state { get; set; }
        public string _city { get; set; }
        public string _street { get; set; }
        public string _apartmentNum { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
    }
}
