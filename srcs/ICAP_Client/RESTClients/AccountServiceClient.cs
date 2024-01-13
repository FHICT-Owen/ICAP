using System.Net.Http.Json;

namespace ICAP_Client.RESTClients
{
    public class AccountServiceClient(HttpClient httpClient, IConfiguration config)
    {
        private readonly string _serverUrl = config["ServerUrl"] ?? throw new InvalidOperationException();

        public async Task SendTokenAsync(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await httpClient.PostAsJsonAsync($"{_serverUrl}/accounts/users", new { });
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task RemoveUserDataAsync(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await httpClient.DeleteAsync($"{_serverUrl}/accounts/users");
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
