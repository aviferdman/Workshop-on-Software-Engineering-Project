using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    class UserManagement
    {
        private ICollection<DataUser> _dataUsers;
        private Authentication _authentication;
        private static readonly Lazy<UserManagement>
        _lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return _lazy.Value; } }

        private UserManagement()
        {
        }
    }
}
