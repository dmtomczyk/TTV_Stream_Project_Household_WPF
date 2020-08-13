using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace STR001.Core
{
    public enum Recurrance
    {
        [Description("Every day.")]
        Daily,

        [Description("Every week.")]
        Weekly,

        [Description("Every 2 weeks.")]
        Fortnightly,

        [Description("Every month.")]
        Monthly,

        [Description("Every 3 months.")]
        Quarterly,

        [Description("Every year.")]
        Annually
    }

    public static class Constants
    {

        /// <summary>
        /// TODO: Break out recurring into it's own DB table
        /// so that EU's can choose their own.
        /// </summary>
        //[Converter
        

    }
}
