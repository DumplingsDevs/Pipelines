using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Application;
using Pipelines.CleanArchitecture.Infrastructure.Commands.Decorators;

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
            .AddDispatcher<ICommandDispatcher>(infrastructureAssembly)
            .WithDecorator(typeof(FluentValidationCommandDecorator<>))
            .WithDecorator(typeof(ValidationCommandDecorator<>))
            .Build();
        
        services.Scan(s => s.FromAssemblies(commandsAssembly)
            .AddClasses(classes =>
                classes.AssignableTo(typeof(IValidator<>)).Where(_ => !_.IsGenericType))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}