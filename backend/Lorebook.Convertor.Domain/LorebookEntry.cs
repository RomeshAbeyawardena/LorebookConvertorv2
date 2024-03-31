namespace Lorebook.Convertor.Domain;

public class LorebookEntry
{
    public Guid? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Text { get; set; }
    public IEnumerable<string> Keys { get; set; } = [];
    public Guid? Category { get; set; }
}