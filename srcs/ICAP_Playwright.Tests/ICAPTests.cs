using System.Runtime.InteropServices.JavaScript;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Newtonsoft.Json.Linq;

namespace ICAP_Playwright.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private string _email;
        private string _password;

        [SetUp]
        public async Task SetupTests()
        {
            var projectRootPath = Directory.GetCurrentDirectory();
            while (projectRootPath != null && !Directory.EnumerateFiles(projectRootPath, "*.csproj", SearchOption.TopDirectoryOnly).Any())
            {
                projectRootPath = Directory.GetParent(projectRootPath)?.FullName;
            }

            var json = File.ReadAllText(Path.Combine(projectRootPath, "appsettings.json"));
            var configuration = JObject.Parse(json);
            _email = configuration["Username"]?.ToString() ?? throw new ArgumentNullException();
            _password = configuration["Password"]?.ToString() ?? throw new ArgumentNullException();
            await Page.GotoAsync("https://icap.odb-tech.com/");
            await Page.WaitForSelectorAsync("header");
        }
        
        [Test]
        public async Task LogInAndGetAccessTokenAndLogOut()
        {
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log In");
            var page1 = await Page.RunAndWaitForPopupAsync(async () =>
            {
                await Page.GetByRole(AriaRole.Button, new() { Name = "Log In" }).ClickAsync();
            });
            await page1.GetByPlaceholder("Email, phone, or Skype").FillAsync(_email);
            await page1.GetByPlaceholder("Email, phone, or Skype").ClickAsync();
            await page1.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();
            await page1.GetByPlaceholder("Password").FillAsync(_password);
            await page1.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            var buttonLocator = Page.Locator("button:text('Ask later')");
            if (await buttonLocator.IsVisibleAsync()) await page1.GetByRole(AriaRole.Button, new() { Name = "Ask later" }).ClickAsync();
            await page1.RunAndWaitForRequestFinishedAsync(async () =>
            {
                await page1.GetByRole(AriaRole.Button, new() { Name = "No" }).ClickAsync();
            });
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log Out");
            await Page.GotoAsync("https://icap.odb-tech.com/user");
            var elementLocator = Page.Locator("#access-token");
            await elementLocator.WaitForAsync();
			var token = await elementLocator.TextContentAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log Out" }).ClickAsync();
            await Page.Locator("[data-test-id=\"testaccount\\@owendebreetjes\\.onmicrosoft\\.com\"]").ClickAsync();
            await Page.GotoAsync("https://icap.odb-tech.com/");
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log In");
            Console.WriteLine("Access Token: " + token);
        }

        [Test]
        public async Task LogInAndClickDeleteAccountData()
        {
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log In");
            var page1 = await Page.RunAndWaitForPopupAsync(async () =>
            {
                await Page.GetByRole(AriaRole.Button, new() { Name = "Log In" }).ClickAsync();
            });
            await page1.GetByPlaceholder("Email, phone, or Skype").FillAsync(_email);
            await page1.GetByPlaceholder("Email, phone, or Skype").ClickAsync();
            await page1.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();
            await page1.GetByPlaceholder("Password").FillAsync(_password);
            await page1.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            var buttonLocator = Page.Locator("button:text('Ask later')");
            if (await buttonLocator.IsVisibleAsync()) await page1.GetByRole(AriaRole.Button, new() { Name = "Ask later" }).ClickAsync();
            await page1.RunAndWaitForRequestFinishedAsync(async () =>
            {
                await page1.GetByRole(AriaRole.Button, new() { Name = "No" }).ClickAsync();
            });
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log Out");
            await Page.GotoAsync("https://icap.odb-tech.com/user");
            await Page.RunAndWaitForRequestFinishedAsync(async () =>
            {
                await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            });
            await Page.Locator("[data-test-id=\"testaccount\\@owendebreetjes\\.onmicrosoft\\.com\"]").ClickAsync();
            await Page.GotoAsync("https://icap.odb-tech.com/");
            await Expect(Page.GetByRole(AriaRole.Banner)).ToContainTextAsync("Log In");
        }
    }
}