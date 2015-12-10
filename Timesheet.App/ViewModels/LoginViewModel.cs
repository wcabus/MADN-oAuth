using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Timesheet.App.Services;
using Timesheet.Domain;
using Task = System.Threading.Tasks.Task;
using IdentityModel;
using IdentityModel.Client;

namespace Timesheet.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly ApiService _apiService;

        private ICommand _loginCommand;

        public LoginViewModel(INavigationService navService, ApiService apiService)
        {
            _navService = navService;
            _apiService = apiService;

            RegisterMessaging();
            CreateCommands();
        }

        private void RegisterMessaging()
        {
        }

        private void CreateCommands()
        {
            _loginCommand = new RelayCommand(async () => await LoginAsync());
        }

        public ICommand LoginCommand => _loginCommand;

        private async Task LoginAsync()
        {
            const string responseType = "id_token token";

            var scopes = $"openid email profile {TimesheetConstants.ApiScope}";
            var nonce = GenerateNonce();

            try
            {
                var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
                var request = new AuthorizeRequest(TimesheetConstants.AuthorizeEndpoint);
                var authUrl = request.CreateAuthorizeUrl(TimesheetConstants.ClientId, responseType, scopes, redirectUri.ToString(), nonce: nonce);
                var requestUri = new Uri(authUrl);

                var result =
                    await
                        WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, redirectUri);

                if (result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    // Successful authentication. 
                    var response = new AuthorizeResponse(result.ResponseData);
                    var userInfoRequest = new UserInfoClient(new Uri(TimesheetConstants.UserInfoEndpoint), response.AccessToken);
                    var userInfo = await userInfoRequest.GetAsync();

                    App.EmployeeId = userInfo.Claims.FirstOrDefault(x => x.Item1 == "name")?.Item2 ?? "";

                    _apiService.Token = response.AccessToken;
                    NavigateToDetailPage();
                }
            }
            catch (Exception ex)
            {
                var dlg = new MessageDialog(ex.Message, "Error");
                await dlg.ShowAsync();
            }
        }

        private void NavigateToDetailPage()
        {
            _navService.NavigateTo("CreateRegistrationView");
        }

        private string GenerateNonce(uint length = 16)
        {
            var rndBuffer = Windows.Security.Cryptography.CryptographicBuffer.GenerateRandom(length);
            return Convert.ToBase64String(rndBuffer.ToArray());
        }
    }
}