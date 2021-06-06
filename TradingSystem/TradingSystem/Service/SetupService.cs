using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    class SetupService
    {
        private static readonly Lazy<SetupService> instanceLazy = new Lazy<SetupService>(() => new SetupService(), true);

        private SetupService()
        {
        }
        public static SetupService Instance => instanceLazy.Value;

        public Result<String> initialize()
        {
            try
            {
                String[] lines = System.IO.File.ReadAllLines(@"init.txt");
                for (int i=0; i<lines.Length; i++)
                {
                    Char[] delimiters = { '(', ',', ')', ';' };
                    String[] command = lines[i].Replace(" ", "").Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    Result<String> result = navigate(command);
                    if (result.IsErr)
                    {
                        return result;
                    }
                }
            } catch { return new Result<string>("something went wrong", true, "invalid input"); }

            return new Result<string>("something went wrong", true, "invalid input");
        }

        private async Task<Result<String>> navigate(String[] command)
        {
            try
            {
                switch (command[0])
                {
                    case "AddGuest":
                        return determine(MarketUserService.Instance.AddGuest());
                    case "Login":
                        return determine(await MarketUserService.Instance.loginAsync(command[1], command[2], ""));
                    case "Logout":
                        return determine(await MarketUserService.Instance.logoutAsync(command[1]));
                }
            } catch { return new Result<string>("bad paramaters", true, "some command has bad paramaters"); }

            return new Result<string>("something went wrong", true, "invalid input");
        }

        private Result<String> determine(String res)
        {
            if (res.Equals())
        }
    }
}
