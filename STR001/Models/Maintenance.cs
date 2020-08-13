using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using STR001.Core;
using STR001.Core.Models;
using static STR001.Core.Constants;

namespace STR001.WPF.Models
{

    public class Maintenance
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
        public Recurrance Period { get; set; }

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


    }
}
