using System;
using System.Collections.Generic;
using System.Text;
using STR001.Core.Models;

namespace STR001.Core.Interfaces
{
    public interface IDataService
    {
        void Upsert(IUnitOfWork STRUnitOfWork, MaintenanceDTO maintenanceToUpsert);

    }
}
