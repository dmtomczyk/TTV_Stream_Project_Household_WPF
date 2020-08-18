using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using STR001.WPF.Models;
using STR001.WPF.ViewModels;
using CommonServiceLocator;
using STR001.Core.Interfaces;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

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

        private UserSettingsViewModel _userSettingsViewModel;
        public UserSettingsViewModel UserSettingsViewModel
        {
            get => _userSettingsViewModel;
            set => Set(ref _userSettingsViewModel, value);
        }

        private bool _isSettingsViewClosed = true;
        public bool IsSettingsViewClosed
        {
            get => _isSettingsViewClosed;
            set => Set(ref _isSettingsViewClosed, value);
        }

        public MainViewModel(IDataService service)
        {
            _service = service;

            MaintenanceVM = ServiceLocator.Current.GetInstance<MaintenanceViewModel>();
        }

        #region Commands

        public ICommand OpenUserSettingsCommand => new RelayCommand(() =>
        {
            UserSettingsViewModel = ServiceLocator.Current.GetInstance<UserSettingsViewModel>();

            IsSettingsViewClosed = !IsSettingsViewClosed;
        });

        #endregion

    }
}
