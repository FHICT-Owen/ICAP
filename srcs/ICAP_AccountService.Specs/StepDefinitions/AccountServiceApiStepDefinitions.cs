using DotNetEnv;
using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace ICAP_AccountService.Specs.StepDefinitions
{
    [Binding]
    public sealed class AccountServiceApiStepDefinitions
    {
        private const string BaseAddress = "http://localhost/";
        public HttpClient Client { get; set; } = null!;
        private HttpResponseMessage Response { get; set; } = null!;

        private readonly string _token;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IRepository<User> _repository;

        public AccountServiceApiStepDefinitions(WebApplicationFactory<Program> factory, IRepository<User> repository)
        {
            var clientId = Environment.GetEnvironmentVariable("ClientId") ?? throw new ArgumentNullException();
            var tenantId = Environment.GetEnvironmentVariable("TenantId") ?? throw new ArgumentNullException();
            var username = Environment.GetEnvironmentVariable("Username") ?? throw new ArgumentNullException();
            var password = Environment.GetEnvironmentVariable("Password") ?? throw new ArgumentNullException();
            var scope = Environment.GetEnvironmentVariable("Scope") ?? throw new ArgumentNullException();
            
            Console.WriteLine(password);
            var app = PublicClientApplicationBuilder.Create(clientId)
                .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
                .Build();

            var result = app.AcquireTokenByUsernamePassword(new []{ scope }, username, password)
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
