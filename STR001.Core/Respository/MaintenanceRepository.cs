using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STR001.Core.Interfaces;
using STR001.Core.Models;

namespace STR001.Core.Respository
{
    public class MaintenanceRepository : Repository<MaintenanceDTO>, IMaintenaceRepository
    {

        private readonly MaintenanceContext _context;

        public MaintenanceRepository(MaintenanceContext context) : base(context)
        {
            this._context = context;
        }
        
        /// <summary>
        /// For illustration purposed only. Refactor before use.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaintenanceDTO GetByIDCustom(long id)
        {
            return _context.Maintenance.Single(item => item.Id == id);
        }

    }
}
