using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using STR001.Core.Utilities;
using Syncfusion.Licensing;

namespace STR001
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region License Key Info

        private readonly string _licenseKey = "MzAyNjI4QDMxMzgyZTMyMmUzME5qaXAwK0FMQ1NDMjhmMmlLcXM2eVRCMnBaRk14UmVmWHI3RWhGTjRkckU9";

        #endregion

        public App()
        {

            SyncfusionLicenseProvider.RegisterLicense(_licenseKey);

            DBUtils.SetupDB();

        }

    }
}
