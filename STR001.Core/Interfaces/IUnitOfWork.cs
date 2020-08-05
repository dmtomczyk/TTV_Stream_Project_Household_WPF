using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using STR001.Core.Respository;

namespace STR001.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        public IMaintenanceRepository Maintenance { get; }

        int Complete();

        IDbContextTransaction BeginTransaction();

    }
}
