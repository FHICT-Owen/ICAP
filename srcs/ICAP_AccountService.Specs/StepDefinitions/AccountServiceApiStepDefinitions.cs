using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using ICAP_AccountService.Specs.Hooks.ICAP_AccountService.Specs.Hooks;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ICAP_AccountService.Specs.StepDefinitions
{
    [Binding]
    public sealed class AccountServiceApiStepDefinitions
    {
        private const string BaseAddress = "http://localhost/";
        public HttpClient Client { get; set; } = null!;
        private HttpResponseMessage Response { get; set; } = null!;
        private User? Entity { get; set; }

        private readonly string _token;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IRepository<User> _repository;

        public AccountServiceApiStepDefinitions(WebApplicationFactory<Program> factory, IRepository<User> repository)
        {
            var projectRootPath = Directory.GetCurrentDirectory();
            while (projectRootPath != null && !Directory.EnumerateFiles(projectRootPath, "*.csproj", SearchOption.TopDirectoryOnly).Any())
            {
                projectRootPath = Directory.GetParent(projectRootPath)?.FullName;
            }

            var json = File.ReadAllText(Path.Combine(projectRootPath, AccountServiceHooks.AppSettingsFile));
            var configuration = JObject.Parse(json);
            var clientId = configuration["ClientId"]?.ToString() ?? throw new ArgumentNullException();
            var tenantId = configuration["TenantId"]?.ToString() ?? throw new ArgumentNullException();
            var username = configuration["Username"]?.ToString() ?? throw new ArgumentNullException();
            var password = configuration["Password"]?.ToString() ?? throw new ArgumentNullException();
            var scopes = configuration["Scopes"]?.ToObject<string[]>();

            var app = PublicClientApplicationBuilder.Create(clientId)
                .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
                .Build();

            var result = app.AcquireTokenByUsernamePassword(scopes, username, password)
                .ExecuteAsync().Result;
            _token = result.AccessToken;
            _factory = factory;
            _repository = repository;
        }

        [Given("I am a client")]
        public void GivenIAmAClient()
        {
            Client = _factory.CreateDefaultClient(new Uri(BaseAddress));
        }

        [Given("the repository has user data")]
        public async Task GivenTheRepositoryHasUserData()
        {
            await _repository.CreateAsync(new User
            {
                Id = "TEST1",
                Name = "Tester 1",
                Email = "tester@onmicrosoft.com",
                CreatedDateTime = DateTimeOffset.Now
            });
        }

        [When("I make a GET request to '(.*)'")]
        public async Task WhenIMakeAGetRequestTo(string endpoint)
        {
            Response = await Client.GetAsync(endpoint);
        }

        [When("I make a POST request to '(.*)'")]
        public async Task WhenIMakeAPostRequestTo(string endpoint)
        {
            var user = new User
            {
                Id = "TEST1",
                Name = "Tester 1",
                Email = "tester@onmicrosoft.com",
                CreatedDateTime = DateTimeOffset.Now
            };
            var content = JsonConvert.SerializeObject(user);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            Response = await Client.PostAsync(endpoint, new StringContent(content));
        }

        [Then("the response status code should be '(.*)'")]
        public void ThenTheResponseStatusCodeIs(int statusCode)
        {
            var expected = (HttpStatusCode)statusCode;
            Assert.Equal(expected, Response.StatusCode);
        }

        [Then("the response json should be a list of user objects")]
        public async Task ThenTheResponseDataShouldBeAList()
        {
            var response = await Response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<User>>(response);
            Assert.IsType<List<User>>(actual);
            Assert.True(actual.Count > 0);
        }

        [Then("the response json should be the created user object")]
        public async Task ThenTheResponseDataShouldBeACreatedUser()
        {
            var response = await Response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<User>(response);
            Assert.NotNull(actual);
        }
    }
}
