using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Timesheet.App.Services
{
    public class ApiService : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        const string BaseUri = "http://localhost:2658/api/";

        public ApiService()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string path)
        {
            var uri = BuildUri(path);
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return ParseJson<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> GetSingleAsync<T>(string path)
        {
            var response = await _httpClient.GetAsync(BuildUri(path));
            response.EnsureSuccessStatusCode();

            return ParseJson<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task CreateRegistrationAsync<T>(Guid employeeId, T registration)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BuildUri($"employees/{employeeId}/registrations"), content);
            response.EnsureSuccessStatusCode();
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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
