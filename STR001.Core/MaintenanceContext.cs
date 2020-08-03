using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using STR001.Core.Models;
using STR001.Core.Respository;

namespace STR001.Core
{
    public class MaintenanceContext : DbContext
    {
        public DbSet<MaintenanceDTO> Maintenance;


    }
}
