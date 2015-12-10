using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Timesheet.App.Services;
using Timesheet.App.Views;

namespace Timesheet.App.ViewModels
{
    public sealed class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Services
            SimpleIoc.Default.Register<ApiService>();

            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register(() => navigationService);

            // Register ViewModels
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<CreateRegistrationViewModel>();
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("CreateRegistrationView", typeof(CreateRegistrationView));

            return navigationService;
        }

        public CreateRegistrationViewModel CreateRegistrationModel => ServiceLocator.Current.GetInstance<CreateRegistrationViewModel>();
        public LoginViewModel LoginModel => ServiceLocator.Current.GetInstance<LoginViewModel>();
    }
}
