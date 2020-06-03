using System;

namespace TodoApi.Server.ForecastsData
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
