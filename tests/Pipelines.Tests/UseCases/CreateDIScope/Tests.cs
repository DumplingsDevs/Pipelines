namespace Pipelines.Tests.UseCases.CreateDIScope;

using Types.WithResponse;
using Types.WithoutResponse;
using Samples;

public class Tests
{
    [Test]
    public async Task VoidDispatcher_CreateDIScope_True_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureVoidPipeline(dependencyContainer, true);
        var dispatcher = dependencyContainer.GetService<IVoidDispatcher>();

        //Act
        await dispatcher.SendAsync(new SampleVoidInput(), new CancellationToken());

        //Assert
        Assert.Pass();
    }

    [Test]
    public async Task VoidDispatcher_CreateDIScope_False_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigureVoidPipeline(dependencyContainer, false);
        var dispatcher = dependencyContainer.GetService<IVoidDispatcher>();

        //Act
        await dispatcher.SendAsync(new SampleVoidInput(), new CancellationToken());

        //Assert
        Assert.Pass();
    }

    [Test]
    public async Task DispatcherWithResult_CreateDIScope_True_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigurePipelineWithResult(dependencyContainer, true);
        var dispatcher = dependencyContainer.GetService<IDispatcherWithResponse>();

        //Act
        var result = await dispatcher.SendAsync(new SampleInputWithResult(), new CancellationToken());
        
        //Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task DispatcherWithResult_CreateDIScope_false_Success()
    {
        //Arrange
        var dependencyContainer = new DependencyContainer();
        ConfigurePipelineWithResult(dependencyContainer, false);
        var dispatcher = dependencyContainer.GetService<IDispatcherWithResponse>();

        //Act
        var result = await dispatcher.SendAsync(new SampleInputWithResult(), new CancellationToken());
        
        //Assert
        Assert.NotNull(result);
    }

    private static void ConfigurePipelineWithResult(DependencyContainer dependencyContainer, bool createDiScope)
    {
        var assembly = typeof(DependencyContainer).Assembly;

        dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputWithResponse<>))
            .AddHandler(typeof(IHandlerWithResponse<,>), assembly)
            .AddDispatcher<IDispatcherWithResponse>(
                new DispatcherOptions()
                {
                    UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation,
                    ThrowExceptionIfHandlerNotFound = true,
                    CreateDIScope = createDiScope
                }, assembly));

        dependencyContainer.BuildContainer();
    }

    private static void ConfigureVoidPipeline(DependencyContainer dependencyContainer, bool createDiScope)
    {
        var assembly = typeof(DependencyContainer).Assembly;

        dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IVoidInput))
            .AddHandler(typeof(IVoidHandler<>), assembly)
            .AddDispatcher<IVoidDispatcher>(
                new DispatcherOptions()
                {
                    UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation,
                    ThrowExceptionIfHandlerNotFound = true,
                    CreateDIScope = createDiScope
                }, assembly));
        dependencyContainer.BuildContainer();
    }
}