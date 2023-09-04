using Pipelines.CleanArchitecture.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.RegisterEndpoints();
app.Run();