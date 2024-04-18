using Lorebook.Convertor.Web.Api.AntiforgeryToken;
using Lorebook.Convertor.Web.Api.AntiforgeryToken.Extensions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Lorebook;
using Lorebook.Convertor.Web.Api.Middleware;
using Lorebook.Convertor.Web.Api.Session;
using Lorebook.Convertor.Web.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApiServices()
    .AddAntiForgeryTokenServices()
    .AddHostedService<SessionLedgerPruningBackgroundService>();
var app = builder.Build();

app
    .UseOwnMiddleware()
    .AddAntiForgeryTokenEndpoints()
    .AddLorebookEndpoints()
    .AddSessionEndpoints();
app.UseAntiforgery();
app.Run();
