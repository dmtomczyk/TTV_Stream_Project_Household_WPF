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
using System.Collections.ObjectModel;

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

            AllMaintenanceItems = _service.GetMaintenances(_locator.UnitOfWork);
        }

        public Guid Id { get; set; }

        private string _subject;
        public string Subject
        {
            get => _subject;
            set => Set(ref _subject, value);
        }

        private string _miscInfo;
        public string MiscInfo
        {
            get => _miscInfo;
            set => Set(ref _miscInfo, value);
        }

        private DateTime? _dateDue;
        public DateTime? DateDue
        {
            get => _dateDue;
            set => Set(ref _dateDue, value);
        }

        private DateTime? _dateLastCompleted;
        public DateTime? DateLastCompleted
        {
            get => _dateLastCompleted;
            set => Set(ref _dateLastCompleted, value);
        }

        private MaintenanceDTO _selectedMaintenanceItem;
        public MaintenanceDTO SelectedMaintenanceItem
        {
            get => _selectedMaintenanceItem;
            set
            {
                if (Set(ref _selectedMaintenanceItem, value))
                {
                    if (value != null)
                    {
                        SetMaintenanceItem(value);
                    }
                }
            }
        }

        private ObservableCollection<MaintenanceDTO> _allMaintenanceItems;
        public ObservableCollection<MaintenanceDTO> AllMaintenanceItems
        {
            get => _allMaintenanceItems;
            set => Set(ref _allMaintenanceItems, value);
        }

        #region Commands

        public ICommand SaveChangesCommand => new RelayCommand(() =>
        {
            // TODO: Create method that returns a new instance of the DTO
            // with the values of the current ViewModel.
            MaintenanceDTO newDTO = GetMaintenaceItem();
            _service.Upsert(_locator.UnitOfWork, newDTO);

            UpdateExistingItems();
        });

        public ICommand DeleteItemCommand => new RelayCommand(() =>
        {
            MaintenanceDTO newDTO = GetMaintenaceItem();
            _service.Delete(_locator.UnitOfWork, newDTO);
        });

        public ICommand GetItemCommand => new RelayCommand(() =>
        {
            //SetMaintenanceItem();
        });

        #endregion

        private void UpdateExistingItems()
        {
            AllMaintenanceItems = _service.GetMaintenances(_locator.UnitOfWork);
        }

        /// <summary>
        /// This method uses the values of an existing <see cref="MaintenanceDTO"/> and sets the properties
        /// on the ViewModel based on it's values..
        /// </summary>
        internal void SetMaintenanceItem(MaintenanceDTO itemToSet)
        {
            //MaintenanceDTO itemToSet = _service.GetMaintenance(_locator.UnitOfWork, new Guid());

            Id = itemToSet.Id;
            Subject = itemToSet.Subject;
            DateDue = itemToSet.DateDue;
            DateLastCompleted = itemToSet.DateLastCompleted;
            MiscInfo = itemToSet.MiscInfo;
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
                Subject = Subject,
                MiscInfo = MiscInfo
            };
        }

    }
}
