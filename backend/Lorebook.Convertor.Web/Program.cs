using Lorebook.Convertor.Web.Api.Lorebook;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.AddLorebookEndpoints();
app.Run();
