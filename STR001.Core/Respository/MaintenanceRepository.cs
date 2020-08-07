using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using STR001.Core.Interfaces;
using STR001.Core.Models;

namespace STR001.Core.Respository
{
    public class MaintenanceRepository : Repository<MaintenanceDTO>, IMaintenanceRepository
    {

        public MaintenanceContext MaintenanceContext => Context as MaintenanceContext;
        //private readonly MaintenanceContext _context;

        public MaintenanceRepository(MaintenanceContext context) : base(context)
        {
            //this._context = context;
        }

        public MaintenanceDTO GetNoTracking(Guid maintenanceId)
        {
            return MaintenanceContext.Maintenance.AsNoTracking().SingleOrDefault(item => item.Id == maintenanceId);
        }
        
    }
}
