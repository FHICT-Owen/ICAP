using System.Net.Http.Json;

namespace ICAP_Client.RESTClients
{
    public class AccountServiceClient(HttpClient httpClient, IConfiguration config)
    {
        private readonly string _serviceUrl = config["ServerUrl"] + "/accounts" ?? throw new InvalidOperationException();

        public async Task SendUserDataAsync()
        {
            await httpClient.PostAsJsonAsync($"{_serviceUrl}/users", new { });
        }

        public async Task<bool> RemoveUserDataAsync()
        {
            var response = await httpClient.DeleteAsync($"{_serviceUrl}/users");
            return response.IsSuccessStatusCode;
        }
    }
}
