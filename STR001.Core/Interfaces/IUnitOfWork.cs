using System;
using System.Collections.Generic;
using System.Text;
using STR001.Core.Respository;

namespace STR001.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        public MaintenanceRepository Maintenance { get; set; }


    }
}
