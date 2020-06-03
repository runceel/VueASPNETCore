using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Server.ForecastsData;

namespace TodoApi.Server.Services
{
    public interface IForecastRepository
    {
        Task<IEnumerable<WeatherForecast>> GetAllAsync();
    }
}
