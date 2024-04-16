using Lorebook.Convertor.Web.Api.AntiforgeryToken;
using Lorebook.Convertor.Web.Api.AntiforgeryToken.Extensions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Lorebook;
using Lorebook.Convertor.Web.Api.Session;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApiServices()
    .AddAntiForgeryTokenServices();
var app = builder.Build();

app
    .AddAntiForgeryTokenEndpoints()
    .AddLorebookEndpoints()
    .AddSessionEndpoints();
app.UseAntiforgery();
app.Run();
