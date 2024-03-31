using System.Text.Json;

namespace Lorebook.Convertor.Domain;

public record Lorebook
{
    public static Lorebook From(DeserialisedLorebook? lorebook)
    {
        var parsedlorebook = new Lorebook
        {
            Categories = lorebook?.Categories,
            Entries = lorebook?.Entries.Select(e => Entry.From(e, lorebook.Categories))
        };

        parsedlorebook.Groupings = LorebookGrouping.Parse(parsedlorebook);

        return parsedlorebook;
    }

    public IEnumerable<Entry>? Entries { get; init; } = [];
    public IEnumerable<Category>? Categories { get; init; } = [];
    public IEnumerable<LorebookGrouping>? Groupings { get; private set; } = [];
}

public class DeserialisedLorebook
{
    public static Lorebook? ParseJson(string fileName, JsonSerializerOptions? options = null)
    {
        options ??= new(JsonSerializerDefaults.Web);

        return Lorebook.From(JsonSerializer
                .Deserialize<DeserialisedLorebook>(File
                    .OpenRead(fileName), options));
    }

    public IEnumerable<LorebookEntry> Entries { get; set; } = [];
    public IEnumerable<Category> Categories { get; set; } = [];
}
