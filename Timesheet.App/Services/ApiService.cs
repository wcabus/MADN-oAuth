using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Windows.Security.Credentials;
using IdentityModel.Client;
using Timesheet.Domain;
using Task = System.Threading.Tasks.Task;

namespace Timesheet.App.Services
{
    public class ApiService : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private TokenResponse _tokenResponse;

        // TODO Check if this IP address matches on your machine using "ipconfig".
        // You'll need the IPv4 address of the Ethernet adapter vEthernet (Internal Ethernet Port Windows Phone Emulator Internal Switch)
        // Also, verify if the API lives at the same location in IIS.
        const string BaseUri = "http://169.254.80.80/Timesheet.Api/api/";

        public ApiService()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public TokenResponse TokenResponse
        {
            set
            {
                _tokenResponse = value;

                if (_tokenResponse != null)
                {
                    // Set the authentication header with the bearer token
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer",
                        _tokenResponse.AccessToken);
                }
                else
                {
                    // Clear the authentication header
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
        }
        
        public async Task<IEnumerable<T>> GetListAsync<T>(string path)
        {
            var uri = BuildUri(path);
            var response = await _httpClient.GetAsync(uri);
            EnsureSuccessStatusCode(response);

            return ParseJson<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> GetSingleAsync<T>(string path)
        {
            var response = await _httpClient.GetAsync(BuildUri(path));
            EnsureSuccessStatusCode(response);

            return ParseJson<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task CreateRegistrationAsync<T>(string employeeId, T registration)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BuildUri($"employees/{employeeId}/registrations"), content);
            EnsureSuccessStatusCode(response);
        }

        public async Task RefreshAccessTokenAsync()
        {
            var tokenClient = new TokenClient(
                TimesheetConstants.TokenEndpoint,
                TimesheetConstants.ClientId,
                "mysupersecretkey");

            var response = await tokenClient.RequestRefreshTokenAsync(_tokenResponse.RefreshToken);
            TokenResponse = response;

            // Store the new TokenResponse in the password vault, refresh token might have changed (depends on the options in the STS).
            StoreTokenInVault(response);
        }

        private void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AccessTokenExpiredException();
            }

            response.EnsureSuccessStatusCode(); // throws an exception if the status code is 4xx or 5xx
        }

        private string BuildUri(string path)
        {
            if (BaseUri.EndsWith("/") && path.StartsWith("/"))
            {
                return $"{BaseUri}{path.Substring(1)}";
            }

            return $"{BaseUri}{path}";
        }

        private T ParseJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string GetTokenFromVault()
        {
            var vault = new PasswordVault();
            try
            {
                // Why not return null or introduce a TryRetrieve instead...
                var pwdc = vault.Retrieve(TimesheetConstants.ClientId, TimesheetConstants.VaultUserName);
                return pwdc.Password;
            }
            catch
            {
                // Couldn't retrieve the credential.
            }

            return null;
        }

        public static void StoreTokenInVault(TokenResponse response)
        {
            RemoveTokenFromVault();

            var vault = new PasswordVault();
            var pwdc = new PasswordCredential(TimesheetConstants.ClientId, TimesheetConstants.VaultUserName, response.Raw);
            vault.Add(pwdc);
        }

        public static void RemoveTokenFromVault()
        {
            var vault = new PasswordVault();
            try
            {
                // Why not return null or introduce a TryRetrieve instead...
                var pwdc = vault.Retrieve(TimesheetConstants.ClientId, TimesheetConstants.VaultUserName);
                vault.Remove(pwdc);
            }
            catch
            {
                // Couldn't retrieve the credential
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
