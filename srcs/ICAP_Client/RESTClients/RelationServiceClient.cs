using System.Net.Http.Json;
using ICAP_Client.Dtos;

namespace ICAP_Client.RESTClients
{
    public class RelationServiceClient(HttpClient httpClient, IConfiguration config)
    {
        private readonly string _serviceUrl = config["ServerUrl"] + "/relations" ?? throw new InvalidOperationException();

        public async Task<List<FriendRequestDto>?> GetFriendRequestsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<FriendRequestDto>>($"{_serviceUrl}/friendrequests") ?? null;
        }

        public async Task<bool> AddNewFriendRequest(FriendRequestDto listing)
        {
            var response = await httpClient.PostAsJsonAsync($"{_serviceUrl}/friendrequests", listing);
            return response.IsSuccessStatusCode;
        }
    }
}
