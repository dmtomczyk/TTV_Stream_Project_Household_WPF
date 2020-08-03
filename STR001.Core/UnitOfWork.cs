using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using STR001.Core.Interfaces;
using STR001.Core.Models;
using STR001.Core.Respository;

namespace STR001.Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MaintenanceContext _maintenanceContext;

        public UnitOfWork()
        {
            _maintenanceContext = new MaintenanceContext();

            Maintenance = new MaintenanceRepository(_maintenanceContext);
        }

        #region Repositories

        public MaintenanceRepository Maintenance { get; set; }

        #endregion

        #region IDisposable Implemenation

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _maintenanceContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        
        public void Save()
        {
            _maintenanceContext.SaveChanges();
        }


    }
}
