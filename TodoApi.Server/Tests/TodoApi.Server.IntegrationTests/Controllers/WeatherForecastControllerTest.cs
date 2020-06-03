using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace TodoApi.Server.IntegrationTests.Controllers
{
    public class WeatherForecastControllerTest : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public WeatherForecastControllerTest(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private HttpClient CreateAuthrizedClient() =>
            _factory.WithWebHostBuilder(
                b => b.ConfigureServices(
                     services =>
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { })))
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

        private HttpClient CreateUnauthorizedClient() => _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        [Fact]
        public async Task Unauthorized()
        {
            var c = CreateUnauthorizedClient();
            var r = await c.GetAsync("/WeatherForecast");
            Assert.Equal(HttpStatusCode.Unauthorized, r.StatusCode);
        }

        [Fact]
        public async Task GetAllForecasts()
        {
            var c = CreateAuthrizedClient();
            var r = await c.GetAsync("/WeatherForecast");
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);

            var json = await r.Content.ReadAsStringAsync();
            var body = JsonSerializer.Deserialize<WeatherForecastResponse[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, })
                    .OrderBy(x => x.Date)
                    .ToArray();

            Assert.Equal(3, body.Length);
            Assert.Equal(new DateTime(2020, 4, 1), body[0].Date);
            Assert.Equal(new DateTime(2020, 4, 2), body[1].Date);
            Assert.Equal(new DateTime(2020, 4, 3), body[2].Date);
        }
    }
}
