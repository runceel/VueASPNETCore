using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Server.ForecastsData;
using TodoApi.Server.Services;
using Xunit;

namespace TodoApi.Server.UnitTests.Services
{
    public class ForecastServiceTest
    {
        private ForecastService _target;
        private Mock<IForecastRepository> _mockForecastRepository;

        public ForecastServiceTest()
        {
            _mockForecastRepository = new Mock<IForecastRepository>();
            _target = new ForecastService(
                _mockForecastRepository.Object
            );
        }

        [Fact]
        public async Task GetWeatherForecastsAsyncTest()
        {
            _mockForecastRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new[]
                {
                    new WeatherForecast { Id = 1, },
                    new WeatherForecast { Id = 2, },
                    new WeatherForecast { Id = 3, },
                });

            var results = await _target.GetWeatherForecastsAsync();
            Assert.Equal(3, results.Count());
            Assert.Equal(1, results.ElementAt(0).Id);
            Assert.Equal(2, results.ElementAt(1).Id);
            Assert.Equal(3, results.ElementAt(2).Id);
        }
    }
}
