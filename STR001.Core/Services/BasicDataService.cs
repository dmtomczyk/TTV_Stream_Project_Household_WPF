using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<MaintenanceDTO> GetMaintenances(IUnitOfWork STRUnitOfWork)
        {
            using (STRUnitOfWork)
            {
                return new ObservableCollection<MaintenanceDTO>(STRUnitOfWork.Maintenance.GetAll());
            }
        }

        public MaintenanceDTO GetMaintenance(IUnitOfWork STRUnitOfWork, Guid maintenanceItemId)
        {
            using (STRUnitOfWork)
            {
                return STRUnitOfWork.Maintenance.Get(maintenanceItemId);
            }
        }

        public bool Delete(IUnitOfWork STRUnitOfWork, MaintenanceDTO maintenanceToDelete)
        {
            using (STRUnitOfWork)
            {
                using (IDbContextTransaction transaction = STRUnitOfWork.BeginTransaction())
                {
                    try
                    {
                        STRUnitOfWork.Maintenance.Remove(maintenanceToDelete);
                        STRUnitOfWork.Complete();
                        transaction.Commit();
                        transaction.Dispose();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        // TODO: Log to file.
                        transaction.Rollback();
                        Console.WriteLine("TODO: Delete() exception thrown.");
                        return false;
                    }
                }
            }
        }

        public void Upsert(IUnitOfWork STRUnitOfWork, MaintenanceDTO maintenanceToUpsert)
        {
            using (STRUnitOfWork)
            {
                using (IDbContextTransaction transaction = STRUnitOfWork.BeginTransaction())
                {
                    try
                    {
                        // TODO: Share on stream the alternatives.
                        //if (STRUnitOfWork.Maintenance.GetAsNoTracking(maintenanceToUpsert.Id) is MaintenanceDTO foundMaintenance)
                        if (STRUnitOfWork.Maintenance.Any(items => items.Id == maintenanceToUpsert.Id))
                        {
                            STRUnitOfWork.Maintenance.Update(maintenanceToUpsert);
                        }
                        else
                        {
                            STRUnitOfWork.Maintenance.Add(maintenanceToUpsert);
                        }

                        int numRowsUpdated = STRUnitOfWork.Complete();
                        transaction.Commit();
                        transaction.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("TODO: Upsert() exception thrown.");
                    }
                }
            }
        }

    }
}
