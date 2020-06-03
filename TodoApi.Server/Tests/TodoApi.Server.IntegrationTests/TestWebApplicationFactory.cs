using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoApi.Server.Data;
using TodoApi.Server.ForecastsData;

namespace TodoApi.Server.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            static void removeServiceDescriptor(IServiceCollection services, Type targetType)
            {
                var descriptors = services.Where(x => x.ServiceType == targetType).ToArray();
                foreach (var x in descriptors)
                {
                    services.Remove(x);
                }
            }

            builder.ConfigureServices(services =>
            {
                removeServiceDescriptor(services, typeof(DbContextOptions<ApplicationDbContext>));
                removeServiceDescriptor(services, typeof(DbContextOptions<ForecastDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Testing"));
                services.AddDbContext<ForecastDbContext>(options =>
                    options.UseInMemoryDatabase("Testing"));

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var forecastDbContext = scope.ServiceProvider.GetRequiredService<ForecastDbContext>();

                    applicationDbContext.Database.EnsureDeleted();
                    forecastDbContext.Database.EnsureDeleted();

                    applicationDbContext.Database.EnsureCreated();
                    forecastDbContext.Database.EnsureCreated();

                    InsertTestData(applicationDbContext, forecastDbContext);
                }
            });
        }

        private void InsertTestData(ApplicationDbContext applicationDbContext, ForecastDbContext forecastDbContext)
        {
            forecastDbContext.WeathreForecasts.AddRange(
                new WeatherForecast { Id = 1, Date = DateTimeOffset.ParseExact("2020/04/01", "yyyy/MM/dd", null), Summary = "s1", TemperatureC = 0 },
                new WeatherForecast { Id = 2, Date = DateTimeOffset.ParseExact("2020/04/02", "yyyy/MM/dd", null), Summary = "s2", TemperatureC = 10 },
                new WeatherForecast { Id = 3, Date = DateTimeOffset.ParseExact("2020/04/03", "yyyy/MM/dd", null), Summary = "s3", TemperatureC = 20 }
            );

            applicationDbContext.SaveChanges();
            forecastDbContext.SaveChanges();
        }
    }
}
