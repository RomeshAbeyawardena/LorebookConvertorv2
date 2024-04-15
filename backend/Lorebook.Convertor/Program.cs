// See https://aka.ms/new-console-template for more information
using Lorebook.Convertor.Domain;
using System.Text.Json;

var path = args.FirstOrDefault();

if (string.IsNullOrWhiteSpace(path))
{
    throw new NullReferenceException();
}

if(!File.Exists(path))
{
    throw new FileNotFoundException("Unable to find file");
}

var lorebook = DeserialisedLorebook.ParseJson(path);

var fileName = Path.GetFileNameWithoutExtension(path);
File.WriteAllText($"{fileName}.json", JsonSerializer.Serialize(lorebook));