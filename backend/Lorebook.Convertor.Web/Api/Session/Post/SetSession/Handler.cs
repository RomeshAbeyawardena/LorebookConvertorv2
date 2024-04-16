using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Handler(IMediator mediator, IDistributedCache distributedCache,
    ApplicationSettings applicationSettings, TimeProvider timeProvider) : IRequestHandler<Command, SessionData>
{
    public async Task<SessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        SessionData sessionData = new();
        DateTimeOffset utcNow;
        if (request.SessionId.HasValue)
        {
            sessionData = await mediator.Send(new Query { SessionId = request.SessionId.Value }, cancellationToken)
                ?? throw new NullReferenceException("Session not found");

            utcNow = timeProvider.GetUtcNow();
            if (sessionData.Expires < utcNow)
            {
                sessionData.Modified = utcNow;
                sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
                await distributedCache.CommitSessionData(sessionData, cancellationToken);
            }

            return sessionData;
        }

        utcNow = timeProvider.GetUtcNow();
        sessionData.SessionId = Guid.NewGuid();
        sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
        sessionData.Created = utcNow;

        var key = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.TokenKey));
        var encryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.EncryptionKey));

        var localNow = timeProvider.GetLocalNow();
        sessionData.WebToken = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Audience = applicationSettings.Audiences.FirstOrDefault(),
            Issuer = applicationSettings.Issuers.FirstOrDefault(),
            IssuedAt = localNow.DateTime,
            NotBefore = localNow.DateTime,
            Expires = localNow.DateTime.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes),
            Subject = new ClaimsIdentity(new List<Claim> { new("Session-Id", sessionData.SessionId.ToString()) }),
            SigningCredentials = new SigningCredentials(key, 
                SecurityAlgorithms.HmacSha512),
            CompressionAlgorithm = CompressionAlgorithms.Deflate,
            EncryptingCredentials = new EncryptingCredentials(encryptionKey, 
                SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512)
        });

        await distributedCache.CommitSessionData(sessionData, cancellationToken);
        return sessionData;
    }
}
