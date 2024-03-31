namespace Lorebook.Convertor.Domain;

public class Entry
{
    public Guid? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Text { get; set; }
    public IEnumerable<string> Keys { get; set; } = [];
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public static Entry From(LorebookEntry entry, IEnumerable<Category>? categories = null)
    {
        return new Entry
        {
            Id = entry.Id,
            DisplayName = entry.DisplayName,
            Text = entry.Text,
            Keys = entry.Keys,
            CategoryId = entry.Category,
            Category = categories?.FirstOrDefault(c => c.Id == entry.Category)
        };
    }
}

