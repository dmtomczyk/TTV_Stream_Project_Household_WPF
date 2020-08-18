using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using STR001.Core;
using STR001.Core.Interfaces;
using STR001.Core.Services;
using STR001.WPF.ViewModels;

namespace STR001.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelBase
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // TODO: Get to this at some point.
                //SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, BasicDataService>();
                SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
            }

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<MaintenanceViewModel>();

            SimpleIoc.Default.Register<UserSettingsViewModel>();
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                // TODO: Research ObjectDisposedException.
                if (SimpleIoc.Default.ContainsCreated<IUnitOfWork>())
                {
                    SimpleIoc.Default.Unregister<IUnitOfWork>();
                    SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
                }
                return ServiceLocator.Current.GetInstance<IUnitOfWork>();
            }
        }

        public MainViewModel Main
        {
            get => SimpleIoc.Default.GetInstance<MainViewModel>();
        }
        
        public MaintenanceViewModel MaintenanceVM
        {
            get => SimpleIoc.Default.GetInstance<MaintenanceViewModel>();
        }

    }
}
