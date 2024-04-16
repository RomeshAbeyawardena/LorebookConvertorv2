﻿using Lorebook.Convertor.Web.Api.Session;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public record Command : IRequest<SessionData>
{
    public Guid? SessionId { get; set; }
}
