using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Interfaces
{
    public interface IStorePermission
    {
        public int GetHierarchy(Guid storeId);

        public void SetHierarchy(Guid storeId, int hierarchy);

        public Permission GetPermission(Guid storeId);

        public void SetPermission(Guid storeId, Permission permission);

        public void AddFounder(Guid storeId);

        public bool GetUserHistory(Guid requestedUserId);

        public bool AddSubject(Guid storeId, Permission permission, IStorePermission subjectStorePermission);

        public bool GetStoreHistory(Guid storeId);

        public bool RemoveSubject(Guid storeId, IStorePermission subjectStorePermission);
    }
}
