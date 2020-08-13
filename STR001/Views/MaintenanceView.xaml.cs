using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Syncfusion.Windows.Shared;

namespace STR001.WPF.Views
{
    /// <summary>
    /// Interaction logic for MaintenaceView.xaml
    /// </summary>
    public partial class MaintenanceView : UserControl
    {
        public MaintenanceView()
        {
            InitializeComponent();
        }

        private void DateTimeEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DateTimeEdit dte)
            {
                if (dte.IsDropDownOpen is false)
                {
                    dte.IsDropDownOpen = true;
                    //dte.OpenPopup();
                }
            }
        }

    }
}
