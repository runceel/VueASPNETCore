using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Server.ForecastsData;
using TodoApi.Server.Services;

namespace TodoApi.Server.Repositories
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly ForecastDbContext _forecastDbContext;

        public ForecastRepository(ForecastDbContext forecastDbContext)
        {
            _forecastDbContext = forecastDbContext ?? throw new ArgumentNullException(nameof(forecastDbContext));
        }

        public async Task<IEnumerable<WeatherForecast>> GetAllAsync() => 
            await _forecastDbContext.WeathreForecasts.ToArrayAsync();
    }
}
