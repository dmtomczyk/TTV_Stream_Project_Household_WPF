using System;
using System.Collections.Generic;
using System.Text;
using STR001.Core.Models;

namespace STR001.Core.Interfaces
{
    public interface IMaintenanceRepository : IRepository<MaintenanceDTO>
    {
        MaintenanceDTO GetNoTracking(Guid maintenanceId);
    }
}
