using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public struct Address
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ApartmentNum { get; set; }
    }
}
