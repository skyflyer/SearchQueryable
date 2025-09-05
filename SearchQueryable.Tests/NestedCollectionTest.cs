using Xunit;

namespace SearchQueryable.Tests;


public class NestedCollectionTest
{
    public readonly IQueryable<Brand> _brands;

    public NestedCollectionTest()
    {
        _brands = new List<Brand>() {
            new() {
                Name = "BMW",
                YearEstablished = 1916,
                Models = [
                    new() { Name = "i3", Year = 2020 },
                    new() { Name = "i4", Year = 2021 },
                    new() { Name = "abcDEF", Year = 2030 }
                ]
            },
            new() {
                Name = "Audi",
                YearEstablished = 1909,
                Models = [
                    new() { Name = "A3", Year = 1995 },
                    new() { Name = "A4", Year = 1996 }
                ]
            }
        }.AsQueryable();
    }

    [Fact]
    public void CanSearchByNestedCollectionIgnoreCase()
    {
        var results = _brands.Search("ABCdef", SearchFlags.LaxMode | SearchFlags.IgnoreCase);
        Assert.Single(results);
        Assert.Contains(_brands.First(), results);
    }

    [Fact]
    public void CanSearchByNestedCollection()
    {
        var results = _brands.Search("A3");
        Assert.Single(results);
        Assert.Equal("Audi", results.First().Name);
    }

    [Fact]
    public void DoesNotFindNonExistentCollectionItem()
    {
        var results = _brands.Search("NonExistent", SearchFlags.LaxMode | SearchFlags.IgnoreCase);
        Assert.Empty(results);
    }

    [Fact]
    public void SearchThroughIntegerProperties()
    {
        var results = _brands.Search(
            "1909",
            SearchFlags.LaxMode | SearchFlags.IgnoreCase);
        Assert.Single(results);
        Assert.Equal("Audi", results.First().Name);
    }

    [Fact]
    public void SearchThroughIntegerPropertiesInNestedCollection()
    {
        var results = _brands.Search(
            "1996",
            SearchFlags.LaxMode | SearchFlags.IgnoreCase);
        Assert.Single(results);
        Assert.Equal("Audi", results.First().Name);
    }
}
