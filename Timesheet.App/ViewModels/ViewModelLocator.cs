using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Timesheet.App.Services;

namespace Timesheet.App.ViewModels
{
    public sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Services
            SimpleIoc.Default.Register<ApiService>();

            // Register ViewModels
            SimpleIoc.Default.Register<CreateRegistrationViewModel>();
        }

        public CreateRegistrationViewModel CreateRegistrationModel
        {
            get { return ServiceLocator.Current.GetInstance<CreateRegistrationViewModel>(); }
        }

    }
}
