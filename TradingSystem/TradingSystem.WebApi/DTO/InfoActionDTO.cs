using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class InfoActionDTO<T>
    {
        public string? Username { get; set; }
        public T Key { get; set; }
    }
}
