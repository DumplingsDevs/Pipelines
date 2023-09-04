using Pipelines.CleanArchitecture.Api;
using Pipelines.CleanArchitecture.Infrastructure.Commands;
using Pipelines.CleanArchitecture.Infrastructure.DomainEvents;
using Pipelines.CleanArchitecture.Infrastructure.Persistance;
using Pipelines.CleanArchitecture.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommands();
builder.Services.AddQueries();
builder.Services.AddDomainEvents();
builder.Services.AddPersistence();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Pipelines Example");
    c.RoutePrefix = string.Empty;
});

app.RegisterEndpoints();
app.Run();