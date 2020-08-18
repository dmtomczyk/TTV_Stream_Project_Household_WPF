using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using STR001.Core.Models;
using STR001.Core.Respository;
using STR001.Core.Utilities;
using static STR001.Core.Constants;

namespace STR001.Core
{
    public class MaintenanceContext : DbContext
    {
        private SqliteConnection _connection;

        #region DbSets

        /// <summary>
        /// TODO: Share on stream that the fix for this being null in MaintenanceViewModel
        /// was literally due to a missing getter/setter :(
        ///     -- Took 25 seconds to fix after taking a fresh look at this.
        /// </summary>
        public DbSet<MaintenanceDTO> Maintenance { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // *Should* resolve to something like this >  C:/ProgramData/stream/Maintenance.db
            string dbPath = Path.Combine(
                path1: Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                path2: "stream",
                path3: "Maintenance.db"
            );

            _connection = DBUtils.CreateDBConnection(dbPath, "STRPassword1!");

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif

            optionsBuilder.UseSqlite(_connection);
            SQLitePCL.Batteries_V2.Init();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            BoolToStringConverter boolConverter = new BoolToStringConverter("false", "true");
            DateTimeToStringConverter dateTimeConverter = new DateTimeToStringConverter();

            #region modelBuilder Entities Here

            modelBuilder.Entity<MaintenanceDTO>(entity =>
            {

                // TODO: Come to implement model in Fluent API
                entity.ToTable("Maintenance");

                entity.Property("TaskName").HasColumnType("VARCHAR");
                entity.Property("TaskDescription").HasColumnType("VARCHAR");
                entity.Property("PeriodString").HasColumnType("VARCHAR");/*.HasConversion(typeof(Recurrance));*/
                entity.Property("StartDate").HasColumnType("DATETIME");
                entity.Property("EndDate").HasColumnType("DATETIME");
                entity.Property("LastCompletedDate").HasColumnType("DATETIME");

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%f', 'now')");

                entity.Property(e => e.DateModified)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%f', 'now')");

            });

            #endregion

        }

        public override void Dispose()
        {
            base.Dispose();

            _connection?.Dispose();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity.GetType().GetProperty("Id") is PropertyInfo p_id &&
                    (null == p_id.GetValue(entry.Entity) || !Guid.TryParse(p_id.GetValue(entry.Entity).ToString(), out Guid resultGuid) || resultGuid == Guid.Empty))
                {
                    p_id.SetValue(entry.Entity, DataFunctions.NewGuidComb());
                }
            }

            ChangeTracker.AutoDetectChangesEnabled = false;
            int result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public SqliteConnection GetConnection()
        {
            return _connection;
        }

    }
}
