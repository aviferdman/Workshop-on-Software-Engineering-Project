using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi
{
    public static class Utils
    {
        public static DateTime ConvertDate(DateTime? dateTime)
        {
            return dateTime == null ? DateTime.Today : DateTime.SpecifyKind(dateTime.Value.ToUniversalTime(), DateTimeKind.Local);
        }
    }
}
