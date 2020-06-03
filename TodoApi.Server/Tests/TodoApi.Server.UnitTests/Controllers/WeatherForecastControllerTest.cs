using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Server.Controllers;
using TodoApi.Server.ForecastsData;
using TodoApi.Server.Services;
using Xunit;

namespace TodoApi.Server.UnitTests.Controllers
{
    public class WeatherForecastControllerTest
    {
        private Mock<ILogger<WeatherForecastController>> _mockLogger;
        private Mock<IForecastService> _mockForecastService;
        private WeatherForecastController _target;
        private DefaultHttpContext _httpContext;
        public WeatherForecastControllerTest()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();
            _mockForecastService = new Mock<IForecastService>();
            _target = new WeatherForecastController(
                _mockLogger.Object,
                _mockForecastService.Object);
            _target.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext = new DefaultHttpContext(),
            };
        }

        [Fact]
        public async Task GetTest_200OK()
        {
            _mockForecastService.Setup(x => x.GetWeatherForecastsAsync())
                .ReturnsAsync(new[]
                {
                    new WeatherForecast { Id = 1, Date = new DateTimeOffset(new DateTime(2000, 1, 1)), Summary = "s1", TemperatureC = 0 },
                    new WeatherForecast { Id = 2, Date = new DateTimeOffset(new DateTime(2000, 2, 1)), Summary = "s2", TemperatureC = 1 },
                    new WeatherForecast { Id = 3, Date = new DateTimeOffset(new DateTime(2000, 3, 1)), Summary = "s3", TemperatureC = 2 },
                });
            var r = await _target.Get();
            Assert.Equal(3, r.Value.Count());

            var firstElement = r.Value.ElementAt(0);
            Assert.True(firstElement.Date == new DateTime(2000, 1, 1) &&
                firstElement.Summary == "s1" &&
                firstElement.TemperatureC == 0);
            var secondElement = r.Value.ElementAt(1);
            Assert.True(secondElement.Date == new DateTime(2000, 2, 1) &&
                secondElement.Summary == "s2" &&
                secondElement.TemperatureC == 1);
            var thirdElement = r.Value.ElementAt(2);
            Assert.True(thirdElement.Date == new DateTime(2000, 3, 1) &&
                thirdElement.Summary == "s3" &&
                thirdElement.TemperatureC == 2);
        }

        [Fact]
        public async Task GetTest_404NotFound()
        {
            _mockForecastService.Setup(x => x.GetWeatherForecastsAsync())
                .ReturnsAsync(Enumerable.Empty<WeatherForecast>());
            var r = await _target.Get();
            Assert.IsType<NotFoundResult>(r.Result);
        }

        [Fact]
        public async Task GetTest_200OKWithEmptyData()
        {
            _httpContext.Request.Headers.Add("DoNotUse404", "true");
            _mockForecastService.Setup(x => x.GetWeatherForecastsAsync())
                .ReturnsAsync(Enumerable.Empty<WeatherForecast>());
            var r = await _target.Get();
            Assert.Empty(r.Value);
        }
    }
}
