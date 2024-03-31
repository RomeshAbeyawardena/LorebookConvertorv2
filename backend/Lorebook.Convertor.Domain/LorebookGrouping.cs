namespace Lorebook.Convertor.Domain;

public record LorebookGrouping()
{
    public static IEnumerable<LorebookGrouping>? Parse(Lorebook? lorebook)
    {
        return lorebook?.Entries?.GroupBy(g => g.CategoryId)
            .Select(g => new LorebookGrouping
        {
            CategoryId = g.Key,
            Category = lorebook.Categories?.FirstOrDefault(c => c.Id == g.Key),
            Entries = g.OrderBy(e => e.DisplayName?.Trim()).ToArray()
        }).OrderBy(c => c.Category?.Name);
    }

    public Guid? CategoryId { get; init; }
    public Category? Category { get; init; }
    public IEnumerable<Entry> Entries { get; init; } = [];
}
