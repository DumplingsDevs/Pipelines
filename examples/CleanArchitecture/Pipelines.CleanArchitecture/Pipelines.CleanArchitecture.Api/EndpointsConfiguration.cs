using Pipelines.CleanArchitecture.Api.Endpoints;

namespace Pipelines.CleanArchitecture.Api;

public static class EndpointsConfiguration
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.CreateToDoEndpoint();
        app.GetToDoEndpoint();
    }
}