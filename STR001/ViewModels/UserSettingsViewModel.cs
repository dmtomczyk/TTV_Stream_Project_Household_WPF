using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using STR001.Core.Interfaces;
using STR001.WPF.Models;

namespace STR001.WPF.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {

        private readonly IDataService _service;

        public UserSettingsViewModel(IDataService service)
        {
            _service = service;

            AllPossibleThemeOptions = new ObservableCollection<string>()
            {
                "Office2016White",
                "Office2016DarkGray"
            };
        }

        private ObservableCollection<string> _allPossibleThemeOptions;
        public ObservableCollection<string> AllPossibleThemeOptions
        {
            get => _allPossibleThemeOptions;
            set => Set(ref _allPossibleThemeOptions, value);
        }

        private string _selectedThemeOption;
        public string SelectedThemeOption
        {
            get => _selectedThemeOption;
            set => Set(ref _selectedThemeOption, value);
        }

        public ICommand SaveSettingsCommand => new RelayCommand(() =>
        {
            UserPrefsJSON userPrefsJSON = GetUserSettingsAsJSONObj();

            string serializedUserSettings = JsonConvert.SerializeObject(userPrefsJSON);

            try
            {
                File.WriteAllText("./UserPrefs.json", serializedUserSettings);
            }
            catch (Exception ex)
            {

            }
        });

        private UserPrefsJSON GetUserSettingsAsJSONObj()
        {
            return new UserPrefsJSON()
            {
                Theme = SelectedThemeOption
            };
        }

    }
}
