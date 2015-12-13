using System;
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
using IdentityModel.Client;
using Timesheet.App.Messages;

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
            MessengerInstance.Register<TryLogonUsingVaultMessage>(this, async msg => await TryLoginUsingVaultAsync());
        }

        private void CreateCommands()
        {
            _loginCommand = new RelayCommand(async () => await LoginAsync());
        }

        public ICommand LoginCommand => _loginCommand;

        private async Task LoginAsync()
        {
            await LoginUsingUsernameAndPasswordAsync();
        }

        private async Task LoginUsingUsernameAndPasswordAsync()
        {
            const string responseType = "code id_token";

            var scopes = $"openid email profile offline_access {TimesheetConstants.ApiScope}";
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
                    // Successful authentication, but we only have the ID token. We need to ask for the access token and refresh tokens now. 
                    var response = new AuthorizeResponse(result.ResponseData);
                    var tokenResponse = await GetAuthTokenAsync(response);

                    ApiService.StoreTokenInVault(tokenResponse);
                    
                    await FinishLoginAsync(tokenResponse);
                }
            }
            catch (Exception ex)
            {
                var dlg = new MessageDialog(ex.Message, "Error");
                await dlg.ShowAsync();
            }
        }

        private async System.Threading.Tasks.Task<bool> TryLoginUsingVaultAsync()
        {
            var raw = ApiService.GetTokenFromVault();
            if (string.IsNullOrEmpty(raw))
            {
                return false;
            }

            var tokenResponse = new TokenResponse(raw);
            if (await FinishLoginAsync(tokenResponse))
            {
                return true;
            }

            ApiService.RemoveTokenFromVault();
            return false;
        }

        private async System.Threading.Tasks.Task<TokenResponse> GetAuthTokenAsync(AuthorizeResponse response)
        {
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

            // Let's get the actual (short lived) access token 
            var tokenClient = new TokenClient(
                            TimesheetConstants.TokenEndpoint,
                            TimesheetConstants.ClientId,
                            "mysupersecretkey");

            return await tokenClient.RequestAuthorizationCodeAsync(response.Code, redirectUri.ToString());
        }

        private async System.Threading.Tasks.Task<bool> FinishLoginAsync(TokenResponse response)
        {
            // Let's retrieve the claims using that access token to fetch the user name we need.
            var userInfoRequest = new UserInfoClient(new Uri(TimesheetConstants.UserInfoEndpoint), response.AccessToken);
            var userInfo = await userInfoRequest.GetAsync();

            if (userInfo.IsError || userInfo.IsHttpError)
            {
                return false;
            }

            App.EmployeeId = userInfo.Claims.FirstOrDefault(x => x.Item1 == "name")?.Item2 ?? "";

            // And remember the access token when calling the API
            _apiService.TokenResponse = response;

            NavigateToDetailPage();
            return true;
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