namespace SearchQueryable.Tests;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int YearEstablished { get; set; }
    public IEnumerable<Model> Models { get; set; } = [];
}
