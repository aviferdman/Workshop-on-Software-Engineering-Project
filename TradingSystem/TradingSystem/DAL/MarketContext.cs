using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TradingSystem.Business.UserManagement;
namespace TradingSystem.DAL
{
    class MarketContext : DbContext
    {
        DbSet<DataUser>
    }
}
