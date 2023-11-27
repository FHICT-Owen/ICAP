using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ICAP_AccountService.Entities;

namespace ICAP_AccountService.Specs.API
{
    public class AccountApi
    {
        private readonly RestClient _client;

        public AccountApi()
        {
            _client = new RestClient("https://localhost:5001");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public async Task<bool> CreateUser(User user)
        {
            var request = new RestRequest("users").AddObject(user);
            var response = await _client.PostAsync(request);
            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<IEnumerable<User>?> GetUsers()
        {
            var request = new RestRequest("users");
            return await _client.GetAsync<IEnumerable<User>>(request);
        }

        public async Task<User?> GetUserById(string id)
        {
            var request = new RestRequest($"users/{id}");
            return await _client.GetAsync<User>(request);
        }

        public async Task<bool> UpdateUser(User user)
        {
            var request = new RestRequest("users").AddObject(user);
            var response = await _client.DeleteAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var request = new RestRequest($"users/{id}");
            var response = await _client.DeleteAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
