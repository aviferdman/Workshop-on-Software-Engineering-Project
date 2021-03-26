using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    class UserManagement
    {
        private ICollection<DataUser> dataUsers;
        private Authentication authentication;
        private static readonly Lazy<UserManagement>
        lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return lazy.Value; } }

        private UserManagement()
        {
        }
    }
}
