using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Application;

namespace Pipelines.CleanArchitecture.Infrastructure.Commands;

public static class Extensions
{
    public static void AddCommands(this IServiceCollection services)
    {
        var infrastructureAssembly = typeof(Extensions).Assembly;
        var commandsAssembly = typeof(ApplicationMarker).Assembly;

        services.AddPipeline()
            .AddInput(typeof(ICommand))
            .AddHandler(typeof(ICommandHandler<>), commandsAssembly)
            .AddDispatcher<ICommandDispatcher>(infrastructureAssembly);
    }
}