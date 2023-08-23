
using Pipelines.Builder.Exceptions;
using Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions;
using Pipelines.Tests.UseCases.TypeOfMissingValidation.Types;

namespace Pipelines.Tests.UseCases.TypeOfMissingValidation;

public class Tests
{
    [Test]
    public void Validate_HandlerWithoutTypeOfStructure_ThrowsException()
    {
        if (EnvVariables.UseReflectionProxyImplementation)
        {
            // skip - we don't have that problem on proxy
            return;
        }
        
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;
        
        var handlerType = typeof(IHandler<>);
        
        Assert.Throws<DispatcherNotRegisteredException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(handlerType, assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_AddInputWithoutTypeOfStructure_ThrowsException()
    {
        if (EnvVariables.UseReflectionProxyImplementation)
        {
            // skip - we don't have that problem on proxy
            return;
        }
        
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;
        
        var inputType = typeof(IInputType);

        Assert.Throws<DispatcherNotRegisteredException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(inputType)
                .AddHandler(typeof(IHandler<>), assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}