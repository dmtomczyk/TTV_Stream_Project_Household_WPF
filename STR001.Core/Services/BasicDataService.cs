using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using STR001.Core.Interfaces;
using STR001.Core.Models;

namespace STR001.Core.Services
{
    /// <summary>
    /// Contain methods for data access and manipulation.
    /// </summary>
    public class BasicDataService : IDataService
    {
        
        public void Upsert(IUnitOfWork STRUnitOfWork, MaintenanceDTO maintenanceToUpsert)
        {
            using (STRUnitOfWork)
            {
                using (IDbContextTransaction transaction = STRUnitOfWork.BeginTransaction())
                {
                    try
                    {
                        if (STRUnitOfWork.Maintenance.Get(maintenanceToUpsert.Id) is MaintenanceDTO foundMaintenaceItem)
                        {
                            STRUnitOfWork.Maintenance.Update(maintenanceToUpsert);
                        }
                        else
                        {
                            STRUnitOfWork.Maintenance.Add(maintenanceToUpsert);
                        }

                        STRUnitOfWork.Complete();
                        transaction.Commit();
                        transaction.Dispose();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine("TODO: Log exceptions to DB / file!");
                    }
                }
            }
        }

    }
}
