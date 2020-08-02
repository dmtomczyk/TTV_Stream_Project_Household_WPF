using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace STR001.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _testBind = "Testing";
        public string TestBind
        {
            get => _testBind;
            set => Set(ref _testBind, value);
        }

    }
}
