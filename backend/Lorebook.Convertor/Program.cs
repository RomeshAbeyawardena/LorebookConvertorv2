// See https://aka.ms/new-console-template for more information
using Lorebook.Convertor.Domain;
using System.Text.Json;

var path = args.FirstOrDefault();

static void DisplayUsage(TextWriter writer, bool displayDescription)
{
    if (displayDescription)
    {
        writer.WriteLine("Converts a Lorebook (*.lorebook) file to a JSON output supported by the journal viewer UI app");
    }
    writer.WriteLine("\tUsage: Lorebook.Convertor.exe [path]");
}

try
{
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new NullReferenceException("A path argument is required");
    }

    if(!path.EndsWith(".lorebook", StringComparison.InvariantCultureIgnoreCase))
    {
        throw new NotSupportedException("The file is not a valid lorebook file");
    }

    if (!File.Exists(path))
    {
        throw new FileNotFoundException("Unable to find file");
    }

    var lorebook = DeserialisedLorebook.ParseJson(path);

    var fileNameWithExt = Path.GetFileName(path);
    var fileName = Path.GetFileNameWithoutExtension(path);
    var basePath = path.Replace(fileNameWithExt, string.Empty);
    Console.WriteLine(basePath);
    File.WriteAllText($"{Path.Combine(basePath, fileName)}.json",
        JsonSerializer.Serialize(lorebook));
}
catch(Exception exception)
{
    var defaultColour = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine("An error occurred: {0}", exception.Message);
    DisplayUsage(Console.Error, true);
    Console.ForegroundColor = defaultColour;
}