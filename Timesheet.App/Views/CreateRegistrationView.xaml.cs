using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Timesheet.App.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Timesheet.App.Messages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Timesheet.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateRegistrationView : Page
    {
        public CreateRegistrationView()
        {
            this.InitializeComponent();
        }

        public CreateRegistrationViewModel Vm => (CreateRegistrationViewModel)DataContext;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Messenger.Default.Send(new InitializeRegistrationViewModelMessage());
        }
    }
}
