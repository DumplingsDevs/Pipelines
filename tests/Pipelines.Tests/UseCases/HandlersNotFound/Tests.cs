using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlersNotFound.Samples;
using Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponse;
using Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponseSync;
using Pipelines.Tests.UseCases.HandlersNotFound.Types.WithResponse;

namespace Pipelines.Tests.UseCases.HandlersNotFound;

public class Tests
{
    [Test]
    public void VoidDispatcher_ThrowExceptionIfHandlerNotFound_True_ThrowsException()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureVoidPipeline(dependencyContainer, true);
        var dispatcher = dependencyContainer.GetService<IVoidDispatcher>();

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await dispatcher.SendAsync(new SampleVoidInput(), new CancellationToken()));
    }
    
    [Test]
    public void SyncVoidDispatcher_ThrowExceptionIfHandlerNotFound_False_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureSyncVoidPipeline(dependencyContainer, false);
        var dispatcher = dependencyContainer.GetService<ISyncVoidDispatcher>();

        //Act & Assert
        Assert.DoesNotThrow(() =>
            dispatcher.SendAsync(new SampleSyncVoidInput()));
    }
    
    [Test]
    public void SyncVoidDispatcher_ThrowExceptionIfHandlerNotFound_True_ThrowsException()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureSyncVoidPipeline(dependencyContainer, true);
        var dispatcher = dependencyContainer.GetService<ISyncVoidDispatcher>();

        //Act & Assert
        Assert.Throws<HandlerNotRegisteredException>(() =>
            dispatcher.SendAsync(new SampleSyncVoidInput()));
    }
    
    [Test]
    public void VoidDispatcher_ThrowExceptionIfHandlerNotFound_False_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureVoidPipeline(dependencyContainer, false);
        var dispatcher = dependencyContainer.GetService<IVoidDispatcher>();

        //Act & Assert
        Assert.DoesNotThrowAsync(async () =>
            await dispatcher.SendAsync(new SampleVoidInput(), new CancellationToken()));
    }
    
    [Test]
    public void DispatcherWithResult_ThrowExceptionIfHandlerNotFound_True_ThrowsException()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigurePipelineWithResult(dependencyContainer, true);
        var dispatcher = dependencyContainer.GetService<IDispatcherWithResponse>();

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await dispatcher.SendAsync(new SampleInputWithResult(), new CancellationToken()));
    }
    
    [Test]
    public void DispatcherWithResult_ThrowExceptionIfHandlerNotFound_False_ThrowsException()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigurePipelineWithResult(dependencyContainer, false);
        var dispatcher = dependencyContainer.GetService<IDispatcherWithResponse>();

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await dispatcher.SendAsync(new SampleInputWithResult(), new CancellationToken()));
    }

    private static void ConfigurePipelineWithResult(DependencyContainer dependencyContainer, bool throwExceptionIfHandlerNotFound)
    {
        var assembly = typeof(DependencyContainer).Assembly;

        dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputWithResponse<>))
            .AddHandler(typeof(IHandlerWithResponse<,>), assembly)
            .AddDispatcher<IDispatcherWithResponse>(
                new DispatcherOptions()
                {
                    UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation,
                    ThrowExceptionIfHandlerNotFound = throwExceptionIfHandlerNotFound
                }, assembly));
        
        dependencyContainer.BuildContainer();
    }
    
    private static void ConfigureVoidPipeline(DependencyContainer dependencyContainer, bool throwExceptionIfHandlerNotFound)
    {
        var assembly = typeof(DependencyContainer).Assembly;

        dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IVoidInput))
            .AddHandler(typeof(IVoidHandler<>), assembly)
            .AddDispatcher<IVoidDispatcher>(
                new DispatcherOptions()
                {
                    UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation,
                    ThrowExceptionIfHandlerNotFound = throwExceptionIfHandlerNotFound
                }, assembly));
        dependencyContainer.BuildContainer();
    }
    
    private static void ConfigureSyncVoidPipeline(DependencyContainer dependencyContainer, bool throwExceptionIfHandlerNotFound)
    {
        var assembly = typeof(DependencyContainer).Assembly;

        dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(ISyncVoidInput))
            .AddHandler(typeof(ISyncVoidHandler<>), assembly)
            .AddDispatcher<ISyncVoidDispatcher>(
                new DispatcherOptions()
                {
                    UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation,
                    ThrowExceptionIfHandlerNotFound = throwExceptionIfHandlerNotFound
                }, assembly));
        dependencyContainer.BuildContainer();
    }
}