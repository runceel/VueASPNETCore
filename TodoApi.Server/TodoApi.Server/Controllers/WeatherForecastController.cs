using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApi.Server.Services;

namespace TodoApi.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecastService _forecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<WeatherForecastResponse>>> Get()
        {
            var results = await _forecastService.GetWeatherForecastsAsync();
            if (!results.Any())
            {
                if (HttpContext.Request.Headers.TryGetValue("DoNotUse404", out var headerValue) && 
                    bool.TryParse(headerValue, out var parsedHeader) &&
                    parsedHeader)
                {
                    return Array.Empty<WeatherForecastResponse>();
                }

                return NotFound();
            }

            return results.Select(x => new WeatherForecastResponse
            {
                Date = x.Date.DateTime,
                Summary = x.Summary,
                TemperatureC = x.TemperatureC,
            }).ToArray();
        }
    }
}
