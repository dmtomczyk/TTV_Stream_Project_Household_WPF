using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace STR001.Core.Models
{
    
    /// <summary>
    /// Contains DTO fields relevant to all things home maintenance.
    /// </summary>
    /// <example>
    /// Air Filter replacement.
    /// </example>
    public class MaintenanceDTO
    {

        /// <summary>
        /// Uniquely identify items by their Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The short name / subject of the maintenance item.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Contains all misc. info.
        /// </summary>
        public string MiscInfo { get; set; }

        /// <summary>
        /// The date the maintenance is required to be done.
        /// </summary>
        public DateTime? DateDue { get; set; }

        /// <summary>
        /// The date the task / maintenance was last completed.
        /// </summary>
        public DateTime? DateLastCompleted { get; set; }

        // TODO: Add other recurring properties here.
        // E.g. 
        // bool IsTaskRecurring
        // TimeSpan RecurrancePeriod

    }

}
