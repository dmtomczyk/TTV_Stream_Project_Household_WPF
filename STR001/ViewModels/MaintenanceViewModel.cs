using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using STR001.Core.Interfaces;
using STR001.Core.Services;
using STR001.WPF.ViewModels;
using CommonServiceLocator;
using STR001.Core.Models;

namespace STR001.WPF.ViewModels
{
    public class MaintenanceViewModel : ViewModelBase
    {
        private readonly IDataService _service;
        private readonly ViewModelLocator _locator;
        
        public MaintenanceViewModel(IDataService service)
        {
            _service = service;
            _locator = App.Current.FindResource("Locator") as ViewModelLocator;
        }

        public string Subject { get; set; }

        public DateTime? DateDue { get; set; }

        public DateTime? DateLastCompleted { get; set; }

        #region Commands

        public ICommand SaveChangesCommand => new RelayCommand(() =>
        {
            // TODO: Create method that returns a new instance of the DTO
            // with the values of the current ViewModel.
            MaintenanceDTO newDTO = GetMaintenaceItem();
            _service.Upsert(_locator.UnitOfWork, newDTO);
        });


        #endregion
        
        private MaintenanceDTO GetMaintenaceItem()
        {
            var rand = new Random();

            return new MaintenanceDTO()
            {
                Id = rand.Next(0, 1000000),
                DateDue = DateDue,
                DateLastCompleted = DateLastCompleted,
                Subject = Subject
            };
        }

    }
}
