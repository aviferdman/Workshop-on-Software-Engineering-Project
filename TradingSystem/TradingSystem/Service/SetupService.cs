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
        private Dictionary<string, string> username_guest;
        string username;
        string password;
        StoreData storeData;

        private SetupService()
        {
            guestnames = new Queue<string>();
            username_guest = new Dictionary<string, string>();
        }
        public static SetupService Instance => instanceLazy.Value;

        public async Task<Result<String>> Initialize()
        {
            try
            {
                var path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName);
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
                        var guest = MarketUserService.Instance.AddGuest();
                        guestnames.Enqueue(guest);
                        return new Result<string>("success", false, "success");
                    case "Login":
                        username = command[1].Trim();
                        password = command[2].Trim();
                        var guestname = username_guest[username];
                        var loginRes = await MarketUserService.Instance.loginAsync(username, password, guestname);
                        return Determine(loginRes);
                    case "Logout":
                        username = command[1].Trim();
                        var logoutRes = await MarketUserService.Instance.logoutAsync(username);
                        return CheckNotNull(logoutRes);
                    case "Signup":
                        var guestusername = guestnames.Dequeue();
                        username = command[1].Trim();
                        password = command[2].Trim();
                        var phone = command[3].Trim();
                        username_guest.Add(username, guestusername);
                        var signupRes = await UserService.Instance.SignupAsync(guestusername, username, password, phone);
                        return Determine(signupRes);
                    case "CreateStore":
                        username = command[2].Trim();
                        storeData = await MarketStoreGeneralService.Instance.CreateStoreAsync(command[1].Trim(), username, command[3].Trim(), command[4].Trim(), command[5].Trim(), command[6].Trim(),
                            command[7].Trim(), command[8].Trim(), command[9].Trim(), command[10].Trim(), command[11].Trim(), command[12].Trim(), command[13].Trim());
                        return CheckNotNull(storeData);
                    case "AddProduct":
                        username = command[7].Trim();
                        ProductData productData = new ProductData(storeData.Id, command[1].Trim(), int.Parse(command[2]), double.Parse(command[3]), double.Parse(command[4]), command[5].Trim());
                        var addProductRes = await MarketProductsService.Instance.AddProduct(productData, storeData.Id, username);
                        return Determine(addProductRes.Mess);
                    case "MakeOwner":
                        var assigneeName = command[1].Trim();
                        var assignerName = command[3].Trim();
                        var makeOwnerRes = await MarketStores.Instance.makeOwner(assigneeName, storeData.Id, assignerName);
                        return Determine(makeOwnerRes);
                    default:
                        return new Result<string>("bad paramaters", true, "some command has bad paramaters");
                }
            } catch(Exception e) { return new Result<string>("bad paramaters", true, "some command has bad paramaters"); }
        }

        private Result<String> Determine(String res)
        {
            if (res.ToLower().Equals("success") || res.Equals("Product added"))
            {
                return new Result<string>("success", false, "success");
            }
            return new Result<string>("something went wrong", true, "action failed");
        }

        private Result<string> CheckNotNull(object s)
        {
            if (s == null)
            {
                return new Result<string>("something went wrong", true, "action failed");
            }
            return new Result<string>("success", false, "success");
        }
    }
}
