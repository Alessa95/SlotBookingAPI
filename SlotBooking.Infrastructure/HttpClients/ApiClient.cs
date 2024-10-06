using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SlotBooking.Infrastructure.HttpClients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient, string username, string password)
        {
            _httpClient = httpClient;
            SetBasicAuthHeader(username, password);
        }

        private void SetBasicAuthHeader(string username, string password)
        {
            string authString = $"{username}:{password}";
            byte[] authBytes = Encoding.ASCII.GetBytes(authString);
            string base64Auth = Convert.ToBase64String(authBytes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task PostAsync(string endpoint, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
