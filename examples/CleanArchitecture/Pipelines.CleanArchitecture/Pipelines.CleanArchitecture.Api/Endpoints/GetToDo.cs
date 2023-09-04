using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Application.Queries;

namespace Pipelines.CleanArchitecture.Api.Endpoints;

public static partial class Endpoint
{
    public static void GetToDoEndpoint(this WebApplication app)
    {
        app.MapGet("/toDo/{id:guid}", async (IQueryDispatcher queryDispatcher, Guid id, CancellationToken token) =>
        {
            var query = new GetToDoQuery(id);
            
            var result = await queryDispatcher.SendAsync(query,token);
            return Results.Ok(result);
        });
    }
}