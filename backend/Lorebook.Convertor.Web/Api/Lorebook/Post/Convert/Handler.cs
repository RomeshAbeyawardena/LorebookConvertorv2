using Lorebook.Convertor.Domain;
using MediatR;
using System.IO;
using System.Text.Json;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public class Handler : IRequestHandler<Command, Response>
{
    public async Task<string> Handle(Command request, CancellationToken cancellationToken)
    {
        if (!request.File.FileName.EndsWith(".lorebook", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NotSupportedException("The file is not a valid lorebook file");
        }

        var lorebook = DeserialisedLorebook.ParseJson(request.File.OpenReadStream());
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync<Domain.Lorebook>(stream, lorebook);
        using var textReader = new StreamReader(stream);
        return await textReader.ReadToEndAsync(cancellationToken);
    }
}
