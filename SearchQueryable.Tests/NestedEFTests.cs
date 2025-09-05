using Microsoft.EntityFrameworkCore;
using SearchQueryable.Tests.Data;
using Xunit;

namespace SearchQueryable.Tests;

public class NestedEFTests
{
    private DbFixture _dbFixture;

    public NestedEFTests()
    {
        _dbFixture = new DbFixture();
    }

    [Fact]
    public void CanSearchForBrand()
    {
        using (var ctx = new SearchQueryableDbContext(_dbFixture.DbContextOptions)) {
            var results = ctx.Brands
                .Include(x => x.Models)
                .Search("bmw");

            Assert.Single(results);
        }
    }

    [Fact]
    public void CanSearchIntegerProperty()
    {
        using (var ctx = new SearchQueryableDbContext(_dbFixture.DbContextOptions)) {
            var results = ctx.Brands
                .Include(x => x.Models)
                .Search("1909"); // year established of Audi
            Assert.Single(results);
        }
    }

    [Fact]
    public void CanSearchForModel()
    {
        using (var ctx = new SearchQueryableDbContext(_dbFixture.DbContextOptions)) {
            var results = ctx.Brands
                .Include(x => x.Models)
                .Search("i3");
            Assert.Single(results);
        }
    }

    [Fact]
    public void CanSearchForModelIgnoreCase()
    {
        using (var ctx = new SearchQueryableDbContext(_dbFixture.DbContextOptions)) {
            var results = ctx.Brands
                .Include(x => x.Models)
                .Search("AbCdEf", SearchFlags.IgnoreCase | SearchFlags.LaxMode);
            Assert.Single(results);
        }
    }
}
