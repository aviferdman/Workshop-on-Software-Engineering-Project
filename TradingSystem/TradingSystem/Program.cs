using System;
using System.Threading;
using TradingSystem.Business;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Service;

namespace TradingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            SetupService.Instance.Initialize();
            var t = Transaction.Instance;
        }
    }
}
