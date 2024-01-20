using System.Net.Http.Json;
using ICAP_Client.Dtos;

namespace ICAP_Client.RESTClients
{
    public class MarketServiceClient(HttpClient httpClient, IConfiguration config)
    {
        private readonly string _serviceUrl = config["ServerUrl"] + "/market" ?? throw new InvalidOperationException();

        public async Task<List<MarketListingDto>?> GetListingsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<MarketListingDto>>($"{_serviceUrl}/listings") ?? null;
        }

        public async Task<bool> AddListingAsync(MarketListingDto listing)
        {
            var response = await httpClient.PostAsJsonAsync($"{_serviceUrl}/listings", listing);
            return response.IsSuccessStatusCode;
        }
    }
}
