using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using STR001.WPF.Models;
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.SfSkinManager;

namespace STR001
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            
            InitializeComponent();

            // De-serializing the user's settings.
            UserPrefsJSON userSettings = JsonConvert.DeserializeObject<UserPrefsJSON>(File.ReadAllText("./UserPrefs.json"));

            // Programmatically set the style of the RibbonWindow based on the user settings..
            SfSkinManager.SetVisualStyle(
                obj: this.RibbonWindow,
                value: userSettings.Theme.GetVisualStyleFromString()
            );

        }

        private bool ValidateUserPrefs(UserPrefsJSON userSettings)
        {
            switch (userSettings.Theme)
            {
                case "Office2016White":
                    return true;
                default:
                    return false;
            }
        }

    }

    public static class UserSettingsExtensions
    {
        public static VisualStyles GetVisualStyleFromString(this string visualStyle)
        {
            switch (visualStyle)
            {
                case "Office2016White":
                    return VisualStyles.Office2016White;

                default:
                    return VisualStyles.Office2016DarkGray;
            }
        }
    }

}
