using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.UserManagement
{
    public class RegisteredAdmin : DataUser
    {
        public RegisteredAdmin(string username, string password, string phone) : base(username, password, phone)
        {
            isAdmin = true;
        }
    }
}
