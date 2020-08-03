using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using STR001.WPF.Models;
using STR001.WPF.ViewModels;
using CommonServiceLocator;
using STR001.Core.Interfaces;

namespace STR001.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _service;

        private MaintenanceViewModel _maintenanceVM;
        public MaintenanceViewModel MaintenanceVM
        {
            get => _maintenanceVM;
            set => Set(ref _maintenanceVM, value);
        }

        public MainViewModel(IDataService service)
        {
            _service = service;

            MaintenanceVM = ServiceLocator.Current.GetInstance<MaintenanceViewModel>();
        }

    }
}
