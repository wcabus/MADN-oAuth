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
            const string responseType = "code id_token"; //implicit flow: "id_token token"

            // Space-separated list of scopes we want to receive
            var scopes = $"openid email profile offline_access {TimesheetConstants.ApiScope}";
            var nonce = GenerateNonce(); // Unique token for the authorization request.

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
                    // Successful authentication, but we only have the ID token. 
                    var response = new AuthorizeResponse(result.ResponseData);

                    // We need to ask for the access token and refresh tokens now.
                    var tokenResponse = await GetAuthTokenAsync(response);

                    // Store the tokens in the password vault
                    ApiService.StoreTokenInVault(tokenResponse);
                    
                    // And finish the login process
                    await FinishLoginAsync(tokenResponse);
                }
            }
            catch (Exception ex)
            {
                var dlg = new MessageDialog(ex.Message, "Error");
                await dlg.ShowAsync();
            }
        }

        /// <summary>
        /// Try logging in using the tokens stored in the password vault.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task<bool> TryLoginUsingVaultAsync()
        {
            var raw = ApiService.GetTokenFromVault();
            if (string.IsNullOrEmpty(raw))
            {
                ApiService.RemoveTokenFromVault();
                return false;
            }

            var tokenResponse = new TokenResponse(raw);
            if (await FinishLoginAsync(tokenResponse))
            {
                return true;
            }

            // If logging on fails, remove the token from the vault.
            ApiService.RemoveTokenFromVault();
            return false;
        }

        private async System.Threading.Tasks.Task<TokenResponse> GetAuthTokenAsync(AuthorizeResponse response)
        {
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

            // Let's get the actual (short lived) access token using the authorization code.
            var tokenClient = new TokenClient(
                            TimesheetConstants.TokenEndpoint,
                            TimesheetConstants.ClientId,
                            "mysupersecretkey");

            return await tokenClient.RequestAuthorizationCodeAsync(response.Code, redirectUri.ToString());
        }

        private async System.Threading.Tasks.Task<bool> FinishLoginAsync(TokenResponse response)
        {
            // Let's retrieve the claims using that access token to fetch the user name we need.
            // Note: the user name is already in our response's access token, but this code demonstrates retrieving all claims.
            var userInfoRequest = new UserInfoClient(new Uri(TimesheetConstants.UserInfoEndpoint), response.AccessToken);
            var userInfo = await userInfoRequest.GetAsync();

            if (userInfo.IsError || userInfo.IsHttpError)
            {
                return false;
            }

            // Set the EmployeeId needed to create time registrations.
            App.EmployeeId = userInfo.Claims.FirstOrDefault(x => x.Item1 == "name")?.Item2 ?? "";

            // Remember the access token when calling the API
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