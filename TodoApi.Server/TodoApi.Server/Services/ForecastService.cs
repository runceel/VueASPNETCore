using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Server.ForecastsData;

namespace TodoApi.Server.Services
{
    public class ForecastService : IForecastService
    {
        private readonly IForecastRepository _forecastRepository;

        public ForecastService(IForecastRepository forecastRepository)
        {
            _forecastRepository = forecastRepository ?? throw new ArgumentNullException(nameof(forecastRepository));
        }

        public Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync() => _forecastRepository.GetAllAsync();
    }
}
