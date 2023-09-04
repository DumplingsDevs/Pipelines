using Microsoft.AspNetCore.Mvc;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

namespace Pipelines.CleanArchitecture.Api.Endpoints;

public static partial class Endpoint
{
    public static void CreateToDoEndpoint(this WebApplication app)
    {
        app.MapPost("/toDo", async (CreateToDoCommand command, ICommandDispatcher commandDispatcher, CancellationToken token) =>
        {
            var result = await commandDispatcher.SendAsync(command,token);
            return Results.Ok(result);
        });
    }
}