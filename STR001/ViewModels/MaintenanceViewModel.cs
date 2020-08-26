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
using static STR001.Core.Constants;
using System.Linq;

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

            AllPossibleRecurrancePeriods = (IEnumerable<Recurrance>)Enum.GetValues(typeof(Recurrance));

            AllMaintenanceItems = _service.GetMaintenances(_locator.UnitOfWork);
        }

        //AllPossibleRecurrancePeriods = new Collection<string>();
        //foreach (Recurrance item in (IEnumerable<Recurrance>) Enum.GetValues(typeof(Recurrance)).AsQueryable().GetEnumerator())
        //{
        //    AllPossibleRecurrancePeriods.Add(item.ToString());
        //}

        public Guid Id { get; set; }

        private IEnumerable<Recurrance> _allPossibleRecurrancePeriods;
        public IEnumerable<Recurrance> AllPossibleRecurrancePeriods
        {
            get => _allPossibleRecurrancePeriods;
            set => Set(ref _allPossibleRecurrancePeriods, value);
        }

        private string _taskName;
        public string TaskName
        {
            get => _taskName;
            set => Set(ref _taskName, value);
        }

        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => Set(ref _taskDescription, value);
        }

        private Recurrance _period;
        public Recurrance Period
        {
            get => _period;
            set => Set(ref _period, value);
        }

        private DateTime? _startDate = null;
        public DateTime? StartDate
        {
            get => _startDate;
            set => Set(ref _startDate, value);
        }

        private DateTime? _endDate = null;
        public DateTime? EndDate
        {
            get => _endDate;
            set => Set(ref _endDate, value);
        }
        
        private DateTime? _lastCompletedDate;
        public DateTime? LastCompletedDate
        {
            get => _lastCompletedDate;
            set => Set(ref _lastCompletedDate, value);
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => Set(ref _dateCreated, value);
        }

        private DateTime _dateModified;
        public DateTime DateModified
        {
            get => _dateModified;
            set => Set(ref _dateModified, value);
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

        public ICommand CreateNewItemCommand => new RelayCommand(() =>
        {
            SelectedMaintenanceItem = new MaintenanceDTO();
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

            TaskName = itemToSet.TaskName;
            TaskDescription = itemToSet.TaskDescription;

            Period = itemToSet.Period;
            
            StartDate = itemToSet.StartDate;
            EndDate = itemToSet.EndDate;
            LastCompletedDate = itemToSet.LastCompletedDate;

            DateCreated = itemToSet.DateCreated;
            DateModified = itemToSet.DateModified;
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

                TaskName = TaskName /*?? "Un-named"*/,
                TaskDescription = TaskDescription,

                Period = Period,

                StartDate = StartDate,
                EndDate = EndDate,
                LastCompletedDate = LastCompletedDate,

                DateCreated = DateCreated,
                DateModified = DateModified,
            };
        }

    }
}
