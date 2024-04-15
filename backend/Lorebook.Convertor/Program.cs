// See https://aka.ms/new-console-template for more information
using Lorebook.Convertor.Domain;
using System.Text.Json;

var path = args.FirstOrDefault();
try
{
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new NullReferenceException();
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
    Console.Error.WriteLine(exception);
}