using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static STR001.Core.Constants;

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

        public Guid Id { get; set; }

        /// <summary>
        /// The name / subject of the given task / maintenance.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// The longer description of the requirements of the task / maintenance.
        /// </summary>
        public string TaskDescription { get; set; }

        /// <summary>
        /// How often this task / maintenance should be addressed.
        /// </summary>
        [NotMapped]
        public Recurrance Period
        {
            get
            {
                return PeriodString.ConvertPeriod();
            }
            set
            {

                PeriodString = value.ConvertPeriod();
            }
        }

        public string PeriodString { get; set; }

        /// <summary>
        /// If set, stores the beginning date of recurrance for
        /// the given Task / maintenance.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// If set, stores the end date of recurrance for
        /// the given Task / maintenance.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// If set, stores the datetime of the most recently
        /// completed task.
        /// </summary>
        public DateTime? LastCompletedDate { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        
    }

    public static class RecurranceExtensions
    {
        public static string ConvertPeriod(this Recurrance recurrance)
        {
            switch (recurrance.ToString())
            {
                case string a when a.Equals("Daily", StringComparison.InvariantCultureIgnoreCase):
                    return "Daily";

                case string a when a.Equals("Weekly", StringComparison.InvariantCultureIgnoreCase):
                    return "Weekly";

                case string a when a.Equals("Fortnightly", StringComparison.InvariantCultureIgnoreCase):
                    return "Fortnightly";

                case string a when a.Equals("Monthly", StringComparison.InvariantCultureIgnoreCase):
                    return "Monthly";

                case string a when a.Equals("Quarterly", StringComparison.InvariantCultureIgnoreCase):
                    return "Quarterly";

                case string a when a.Equals("Annually", StringComparison.InvariantCultureIgnoreCase):
                    return "Annually";

                default:
                    return "Weekly";
            }
        }
    }

    public static class StringExtensions
    {

        public static Recurrance ConvertPeriod(this string periodString)
        {
            switch (periodString)
            {
                case "Daily":
                    return Recurrance.Daily;

                case "Weekly":
                    return Recurrance.Weekly;

                case "Fortnightly":
                    return Recurrance.Fortnightly;

                case "Monthly":
                    return Recurrance.Monthly;

                case "Quarterly":
                    return Recurrance.Quarterly;

                case "Annually":
                    return Recurrance.Annually;

                default:
                    return Recurrance.Weekly;
            }
        }

    }

}
