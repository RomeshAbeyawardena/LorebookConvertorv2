using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Handler(IMediator mediator, IDistributedCache distributedCache, IHttpContextAccessor httpContextAccessor,
    ApplicationSettings applicationSettings, TimeProvider timeProvider) : IRequestHandler<Command, SessionData>
{
    private string? CreateToken(SessionData sessionData)
    {
        var key = new SymmetricSecurityKey(
            Convert.FromBase64String(applicationSettings.TokenKey 
                ?? throw new ConfigurationMissingException(nameof(applicationSettings.TokenKey))));
        var encryptionKey = new SymmetricSecurityKey(
            Convert.FromBase64String(applicationSettings.EncryptionKey
                ?? throw new ConfigurationMissingException(nameof(applicationSettings.EncryptionKey))
            ));

        var localNow = timeProvider.GetLocalNow();

        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
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
    }

    private async Task RenewSession(SessionData sessionData, CancellationToken cancellationToken)
    {
        var utcNow = timeProvider.GetUtcNow();
        sessionData.Modified = utcNow;
        sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
        sessionData.WebToken = CreateToken(sessionData);
        await distributedCache.CommitSessionData(sessionData, cancellationToken);
    }

    public async Task<SessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        SessionData sessionData = new();
        DateTimeOffset utcNow;
        if (request.SessionId.HasValue)
        {
            sessionData = await mediator.Send(new Query { SessionId = request.SessionId.Value }, 
                cancellationToken)
                ?? throw new NullReferenceException("Session not found");

            if (!sessionData.IsValid(timeProvider))
            {
                await RenewSession(sessionData, cancellationToken);
            }

            return sessionData;
        }
        //no requested valid session? Use existing session if it exists
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var session = httpContext.GetSessionData();
            if(session != null && !session.IsValid(timeProvider))
            {
                await RenewSession(session, cancellationToken);
                return session;
            }
        }

        utcNow = timeProvider.GetUtcNow();
        sessionData.SessionId = Guid.NewGuid();
        sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
        sessionData.Created = utcNow;
        sessionData.WebToken = CreateToken(sessionData);

        await distributedCache.CommitSessionData(sessionData, cancellationToken);
        return sessionData;
    }
}
