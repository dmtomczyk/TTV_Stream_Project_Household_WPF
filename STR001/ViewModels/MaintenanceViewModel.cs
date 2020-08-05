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
using STR001.Core;

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

        public Guid Id { get; set; }

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

        /// <summary>
        /// This method uses the values of an existing <see cref="MaintenanceDTO"/> and sets the properties
        /// on the ViewModel based on it's values..
        /// </summary>
        internal void SetMaintenanceItem(MaintenanceDTO itemToSet)
        {
            Id = itemToSet.Id;
            Subject = itemToSet.Subject;
            DateDue = itemToSet.DateDue;
            DateLastCompleted = itemToSet.DateLastCompleted;
        }
        
        /// <summary>
        /// This method creates a new <see cref="MaintenanceDTO"/> from the current values
        /// on the ViewModel, and returns it.
        /// </summary>
        /// <returns>A new MaintenanceDTO w/ values from the VM.</returns>
        private MaintenanceDTO GetMaintenaceItem()
        {
            return new MaintenanceDTO()
            {
                Id = Id,
                DateDue = DateDue,
                DateLastCompleted = DateLastCompleted,
                Subject = Subject
            };
        }

    }
}
