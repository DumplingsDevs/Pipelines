using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder;
using Pipelines.Builder.Interfaces;

namespace Pipelines;

public static class Extension
{
    public static IAddInputBuilder AddPipeline(this IServiceCollection service)
    {
        return new PipelineBuilder(service);
    }
}




//ICommandDisptcher
//ICommandHandler
//ICommand
//ICommandResult

//IQueryDisptcher
//IQueryHandler
//IQuery
//IQuery

//LoggingPipelineBehaviour

/*
        services.AddPipeline(List<Assembly>)
            .AddDispatcher<ICommandDisptcher>(nameof(ICommandDisptcher))
            .AddHandler<ICommandHandler>(nameof(ICommandHandler.HandleAsync))
            .AddInput<ICommand>()
            .AddResult<ICommandResult>()
            .AddPipelineBehaviour<LoggingPipelineBehaviour>()
            .AddPipelineBehaviour<LoggingPipelineBehaviour>();

*/