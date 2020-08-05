using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STR001.Core.Interfaces;
using STR001.Core.Models;

namespace STR001.Core.Respository
{
    public class MaintenanceRepository : Repository<MaintenanceDTO>, IMaintenanceRepository
    {

        private readonly MaintenanceContext _context;

        public MaintenanceRepository(MaintenanceContext context) : base(context)
        {
            this._context = context;
        }
        
    }
}
