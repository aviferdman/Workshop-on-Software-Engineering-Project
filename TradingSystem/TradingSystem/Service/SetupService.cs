using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class SetupService
    {
        private static readonly Lazy<SetupService> instanceLazy = new Lazy<SetupService>(() => new SetupService(), true);
        private Queue<String> guestnames;

        private SetupService()
        {
            guestnames = new Queue<string>();
        }
        public static SetupService Instance => instanceLazy.Value;

        public async Task<Result<String>> Initialize()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                string fileName = "init.txt";
                String[] lines = File.ReadAllLines($@"{path}\{fileName}");
                for (int i=0; i<lines.Length; i++)
                {
                    Char[] delimiters = { '(', ',', ')', ';' };
                    String[] command = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    Result<String> result = await Navigate(command);
                    if (result.IsErr)
                    {
                        return result;
                    }
                }
            } catch { return new Result<string>("something went wrong", true, "invalid input"); }

            return new Result<string>("success", false, "initialization succeeded");
        }

        private async Task<Result<String>> Navigate(String[] command)
        {
            try
            {
                switch (command[0].Trim())
                {
                    case "AddGuest":
                        guestnames.Enqueue(MarketUserService.Instance.AddGuest());
                        return new Result<string>("success", false, "success");
                    case "Login":
                        return Determine(await MarketUserService.Instance.loginAsync(command[1].Trim(), command[2].Trim(), guestnames.Dequeue()));
                    case "Logout":
                        String res = await MarketUserService.Instance.logoutAsync(command[1].Trim());
                        if (res == null)
                        {
                            return new Result<string>("something went wrong", true, "invalid input");
                        }
                        return new Result<string>("success", false, "success");
                    case "Signup":
                        return Determine(await UserService.Instance.SignupAsync(guestnames.Dequeue(), command[1].Trim(), command[2].Trim(), command[3].Trim()));
                    case "CreateStore":
                        StoreData storeData = await MarketStoreGeneralService.Instance.CreateStoreAsync(command[1].Trim(), command[2].Trim(), command[3].Trim(), command[4].Trim(), command[5].Trim(), command[6].Trim(),
                            command[7].Trim(), command[8].Trim(), command[9].Trim(), command[10].Trim(), command[11].Trim(), command[12].Trim(), command[13].Trim());
                        if (storeData == null)
                        {
                            return new Result<string>("something went wrong", true, "invalid input");
                        }
                        return new Result<string>("success", false, "success");
                    case "AddProduct":
                        ProductData productData = new ProductData(command[1].Trim(), int.Parse(command[2]), double.Parse(command[3]), double.Parse(command[4]), command[5].Trim());
                        return Determine(MarketProductsService.Instance.AddProduct(productData, Guid.Parse(command[6]), command[7].Trim()).Result.Mess);
                    default:
                        return new Result<string>("bad paramaters", true, "some command has bad paramaters");
                }
            } catch { return new Result<string>("bad paramaters", true, "some command has bad paramaters"); }
        }

        private Result<String> Determine(String res)
        {
            if (res.Equals("success") || res.Equals("Product added"))
            {
                return new Result<string>("success", false, "success");
            }
            return new Result<string>("something went wrong", true, "action failed");
        }
    }
}
