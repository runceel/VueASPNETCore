using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Server.ForecastsData;

namespace TodoApi.Server.UnitTests.Repositories
{
    public class ForecastDbContextFixture : IDisposable
    {
        public ForecastDbContext ForecastDbContext { get; }
        public ForecastDbContextFixture()
        {
            ForecastDbContext = new ForecastDbContext(new DbContextOptionsBuilder<ForecastDbContext>()
                .UseInMemoryDatabase("unit testing")
                .Options);

            // create data for test
            ForecastDbContext.Database.EnsureCreated();
            ForecastDbContext.WeathreForecasts.AddRange(
                new WeatherForecast { Id = 1, Date = DateTimeOffset.ParseExact("2020/04/01", "yyyy/MM/dd", null), Summary = "s1", TemperatureC = 0 },
                new WeatherForecast { Id = 2, Date = DateTimeOffset.ParseExact("2020/04/02", "yyyy/MM/dd", null), Summary = "s2", TemperatureC = 10 },
                new WeatherForecast { Id = 3, Date = DateTimeOffset.ParseExact("2020/04/03", "yyyy/MM/dd", null), Summary = "s3", TemperatureC = 20 }
            );
            ForecastDbContext.SaveChanges();
        }

        public void Dispose()
        {
            ForecastDbContext.Database.EnsureDeleted();
        }
    }
}
