using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Server.ForecastsData;
using TodoApi.Server.Repositories;
using Xunit;

namespace TodoApi.Server.UnitTests.Repositories
{
    public class ForecastRepositoryTest : IClassFixture<ForecastDbContextFixture>
    {
        private ForecastDbContext _context;
        private ForecastRepository _target;

        public ForecastRepositoryTest(ForecastDbContextFixture forecastDbContextFixture)
        {
            _context = forecastDbContextFixture.ForecastDbContext;
            _target = new ForecastRepository(_context);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var results = await _target.GetAllAsync();
            Assert.NotNull(results);
            Assert.Equal(3, results.Count());

            Assert.Equal(1, results.ElementAt(0).Id);
            Assert.Equal(2, results.ElementAt(1).Id);
            Assert.Equal(3, results.ElementAt(2).Id);
        }
    }
}
