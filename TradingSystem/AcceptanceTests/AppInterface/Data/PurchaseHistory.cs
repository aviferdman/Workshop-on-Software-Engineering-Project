using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class PurchaseHistory : IEnumerable<PurchaseHistoryRecord>
    {
        public PurchaseHistory(IEnumerable<PurchaseHistoryRecord> records)
        {
            Records = records;
        }

        public IEnumerable<PurchaseHistoryRecord> Records { get; }

        public IEnumerator<PurchaseHistoryRecord> GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Records).GetEnumerator();
        }
    }
}
