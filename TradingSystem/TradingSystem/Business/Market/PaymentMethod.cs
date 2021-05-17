using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public interface PaymentMethod
    {
        public String GetCardNumber();
        public String GetMonth();
        public String GetYear();
        public String GetHolderName();
        public String GetCVV(); 
        public String GetHolderId();
    }
}
