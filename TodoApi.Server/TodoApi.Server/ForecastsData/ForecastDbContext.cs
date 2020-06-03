using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TodoApi.Server.ForecastsData
{
    public class ForecastDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeathreForecasts { get; set; }

        public ForecastDbContext()
        {
        }

        public ForecastDbContext(DbContextOptions<ForecastDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>(b =>
            {
                b.Property(x => x.Id);
                b.HasKey(x => x.Id);
                b.Property(x => x.TemperatureC);
                b.Property(x => x.Summary);
                b.Property(x => x.Date);
            });
        }
    }
}