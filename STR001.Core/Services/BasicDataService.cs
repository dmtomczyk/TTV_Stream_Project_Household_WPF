using System;
using System.Collections.Generic;
using System.Text;
using STR001.Core.Interfaces;
using STR001.Core.Models;

namespace STR001.Core.Services
{
    /// <summary>
    /// Contain methods for data access and manipulation.
    /// </summary>
    public class BasicDataService : IDataService
    {


        public bool Upsert(IUnitOfWork STRUnitOfWork, MaintenanceDTO maintenanceToUpsert)
        {
            using (STRUnitOfWork)
            {
                if (STRUnitOfWork.Maintenance.GetByID(123) is MaintenanceDTO foundMaintenaceItem)
                {
                    STRUnitOfWork.Maintenance.Update(maintenanceToUpsert);
                }
                else
                {
                    STRUnitOfWork.Maintenance.Insert(maintenanceToUpsert);
                }

                return true;
            }
        }

    }
}
