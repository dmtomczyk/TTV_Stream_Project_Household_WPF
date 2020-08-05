using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using STR001.Core.Interfaces;
using STR001.Core.Models;
using STR001.Core.Respository;

namespace STR001.Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MaintenanceContext _context;

        public UnitOfWork()
        {
            _context = new MaintenanceContext();

            Maintenance = new MaintenanceRepository(_context);
        }

        #region Repositories

        public IMaintenanceRepository Maintenance { get; set; }

        #endregion

        #region IDisposable Implemenation

        //private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            _context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        #endregion

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Save()
        {
            _context.SaveChanges();
        }


    }
}
