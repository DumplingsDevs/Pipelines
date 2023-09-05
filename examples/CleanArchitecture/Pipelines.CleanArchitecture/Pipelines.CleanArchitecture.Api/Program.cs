using Pipelines.CleanArchitecture.Api;
using Pipelines.CleanArchitecture.Api.Extensions;
using Pipelines.CleanArchitecture.Infrastructure.Commands;
using Pipelines.CleanArchitecture.Infrastructure.DomainEvents;
using Pipelines.CleanArchitecture.Infrastructure.Persistance;
using Pipelines.CleanArchitecture.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioningConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCommands();
builder.Services.AddQueries();
builder.Services.AddDomainEvents();
builder.Services.AddPersistence();

var app = builder.Build();
DatabaseCreator.CreateDatabaseSchema(app.Services);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Pipelines Example");
    c.RoutePrefix = string.Empty;
});

app.RegisterEndpoints();
app.Run();