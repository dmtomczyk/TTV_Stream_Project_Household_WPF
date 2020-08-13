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

    /// Monthly:
    ///     Test Smoke Detectors
    ///     Deep Cleaning
    ///     Inspect Fire Extinguishers
    ///     Clean Garbage Disposal
    ///     Unclog Drains
    ///     Clean the stovetop fans & filter
    ///     Replace HVAC Filter (1 / 3 months)

    /// Seasonally:
    /// 
    ///     Spring:
    ///         Wash Out windows and siding
    ///         Clean gutters and downsprouts
    ///         pump the septic tank
    ///         inspect roof and chimney for any damage or leaks
    ///         service air conditioning system
    ///         apply pre-emergent to lawn
    ///         re-seal the deck, fence, other woodworks
    ///         inpsect driveway and other exterior pathways
    ///         inspect sprinkler heads, test system
    ///         spray for mosquitos and other bugs
    ///         repair damaged screen doors and windows
    ///         sharpen lawnmower blades
    ///         
    ///     Summer:
    ///         clean the grill and refill propane tank
    ///         mulch garden beds
    ///         exterior paint touch-ups
    ///         inspect and clean dryer vent
    ///         clean refridgerator coils
    ///         clean bathroom vent fans
    ///         test home alarm
    ///         sanitize trash and recycle bin
    ///         fertilize the lawn
    ///     
    ///     Fall:
    ///         service heating system
    ///         schedule a chimney sweep
    ///         put outdoor furniture and grill into storage
    ///         seal window cracks and doors
    ///         turn off outdoor water
    ///         winterize sprinkler system
    ///         rake leaves
    ///         clena gutters and downspouts
    ///         overseed and aerate the lawn
    ///         ensure pipes are well insulated
    ///         check attic vents
    ///         
    ///     Winter:
    ///         prepare for a storm
    ///         protect entryways (mats / weatherstripping)
    ///         check insulation and add to areas that need more
    ///         protect your AC unit with a piece of plywood
    ///         insulate hot water tank
    ///         purchase a humidifier
    ///         secure steps and handrails
    ///         install storm windows and doors
    ///         remove window screens
    ///         set heat to 55 or higher
    ///         

    ///      -- Interior
    ///          ~ HVAC (Heating / Ventilation / Air Conditioning
    ///              + Air Filter Replacements
    ///          ~ Lighting
    ///          ~ Security
    ///          ~ 
    ///
    ///      -- Exterior
    ///          ~ Landscaping
    ///          ~ Lighting
    ///          ~ Security (Locks / Cameras)
    ///          ~ Sprinkler ? Water
    ///         ~ Aesthetics (Paint / Flags / Welcome Mat)

}
