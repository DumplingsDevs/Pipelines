using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Models;
using Pipelines.Generators.Extensions;
using Pipelines.Generators.Validators.CrossValidation;
using Pipelines.Generators.Validators.Dispatcher;

namespace Pipelines.Generators.Builders;

internal class DispatcherBuilder
{
    private readonly StringBuilder _builder = new();
    private readonly PipelineConfig _pipelineConfig;
    private readonly GeneratorExecutionContext _context;

    private readonly IMethodSymbol _dispatcherMethod;

    public DispatcherBuilder(PipelineConfig pipelineConfig, GeneratorExecutionContext context)
    {
        _pipelineConfig = pipelineConfig;
        _context = context;

        DispatcherParameterConstraintValidator.Validate(_pipelineConfig.DispatcherType);
        CrossValidateResultTypes.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);
        CrossValidateParameters.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);
        
        _dispatcherMethod = _pipelineConfig.DispatcherType.GetMembers().OfType<IMethodSymbol>().First();
    }

    public string Build()
    {
        BuildNamespaces();
        //TO DO - add info that file is generated (good practise)
        BuildClassDefinition();
        AddLine("{");
        BuildRequestHandlerBase();
        BuildRequestHandlerWrapper();
        AddLine("private readonly IServiceProvider _serviceProvider;");
        AddLine("private readonly Dictionary<Type, PipelinesRequestHandlerBase> _handlers = new();");
        BuildConstructor();
        BuildDispatcherHandlerMethod(_dispatcherMethod);
        AddLine("}");

        return _builder.ToString();
    }

    private void BuildNamespaces()
    {
        AddLine("using System;");
        AddLine("using System.Linq;");
        AddLine("using Pipelines.Builder.HandlerWrappers;");
        AddLine("using Microsoft.Extensions.DependencyInjection;");
        AddLine("using Pipelines.Exceptions;");
    }

    private void BuildRequestHandlerBase()
    {
        AddLine(@"
private abstract class PipelinesRequestHandlerBase
{
    internal abstract Task<object> Handle(object request, CancellationToken cancellationToken, IServiceProvider serviceProvider);
}");
    }

    private void BuildRequestHandlerWrapper()
    {
        AddLine(@"
private class PipelinesRequestHandlerWrapperImpl<TRequest, TResponse> : PipelinesRequestHandlerBase
    where TRequest : IRequest<TResponse> where TResponse : class
//TO DO: Probably TResponse don't need to be class constraint
{
    internal override async Task<object> Handle(object request, CancellationToken cancellationToken, IServiceProvider serviceProvider) =>
        await Handle((IRequest<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

    private async Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return await handler.HandleAsync((TRequest)request, cancellationToken);
    }
}");
    }

    private void BuildClassDefinition()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();

        AddLine($"public class {dispatcherInterface}Implementation : {_pipelineConfig.DispatcherType}");
    }

    private void BuildConstructor()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();
        var dispatcherResults = _dispatcherMethod.GetMethodResults();
        var wrapperGenericString = GenerateCommasForGenericParameters(dispatcherResults.Count);
            
        AddLine($"public {dispatcherInterface}Implementation(IServiceProvider serviceProvider, IHandlersRepository handlersRepository)");
        AddLine("{");
        AddLine("_serviceProvider = serviceProvider;");
        AddLine("var handlerTypes = handlersRepository.GetHandlers();");
        AddLine("foreach (var handlerType in handlerTypes)");
        AddLine("{");
        AddLine("var genericArguments = handlerType.GetInterfaces()");
        AddLine($".Single(i => i.GetGenericTypeDefinition() == typeof({_pipelineConfig.HandlerType}))");
        AddLine(".GetGenericArguments();");
        AddLine("var requestType = genericArguments[0];");
        AddLine($"var wrapperType = typeof(PipelinesRequestHandlerWrapperImpl<{wrapperGenericString}>).MakeGenericType(genericArguments);");
        AddLine(@"var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($""Could not create wrapper type for {requestType}"");");
        AddLine("_handlers[requestType] = (PipelinesRequestHandlerBase)wrapper;");
        AddLine("}");
        AddLine("}");
    }

    private void BuildDispatcherHandlerMethod(IMethodSymbol dispatcherHandlerMethod)
    {
        var handlerMethod = _pipelineConfig.HandlerType.ConstructedFrom.GetMembers().OfType<IMethodSymbol>()
            .First();
        var dispatcherResults = dispatcherHandlerMethod.GetMethodResults();
        var hasMultipleResults = dispatcherResults.Count > 1;
        var hasResponse = dispatcherResults.Count > 0;
        var asyncModifier = handlerMethod.IsAsync() ? "await" : "";
        
        AddLine("public",
            AsyncModified(dispatcherHandlerMethod),
            GenerateMethodReturnType(dispatcherHandlerMethod),
            dispatcherHandlerMethod.Name,
            GetMethodConstraint(dispatcherHandlerMethod),
            $"({GenerateMethodParameters(dispatcherHandlerMethod)})",
            GetConstraints(dispatcherHandlerMethod));

        AddLine("{");
        AddLine("var requestType = request.GetType();");
        AddLine("if (!_handlers.TryGetValue(requestType, out var handlerWrapper))");
        AddLine("{");
        AddLine("throw new HandlerNotRegisteredException(requestType);");
        AddLine("}");
        AddLine("var result = await handlerWrapper.Handle(request, token, _serviceProvider);");
        AddLine("return result as TResult;");
        AddLine(CallHandlerWrapperTemplate(hasResponse, hasMultipleResults, asyncModifier, dispatcherHandlerMethod, dispatcherResults));
        AddLine("}");
    }

    public string CallHandlerWrapperTemplate(bool hasResponse, bool hasMultipleResults, string @await, IMethodSymbol dispatcherMethod, List<ITypeSymbol> dispatcherResults)
    {
        if (hasResponse && !hasMultipleResults)
        {
            return SingleResponseHandlerWrapper(@await, dispatcherMethod, dispatcherResults);
        }

        if (hasResponse && hasMultipleResults)
        {
            return MultipleResponsesHandlerWrapper(@await, dispatcherMethod, dispatcherResults);
        }
        else
        {
            return NoneResponseHandlerWrapper(@await, dispatcherMethod, dispatcherResults);
        }
    }

    private string NoneResponseHandlerWrapper(string @await, IMethodSymbol dispatcherMethod, List<ITypeSymbol> dispatcherResults)
    {
        return $@"{@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider)";
    }

    private string MultipleResponsesHandlerWrapper(string @await, IMethodSymbol dispatcherMethod, List<ITypeSymbol> dispatcherResults)
    {
        return $@"var result = {@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider);
{GenerateMultipleArgumentReturn(dispatcherResults)}";
    }

    private string SingleResponseHandlerWrapper(string @await, IMethodSymbol dispatcherMethod, List<ITypeSymbol> dispatcherResults)
    {
        return $@"var result = {@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider);
{GenerateSingleArgumentReturn(dispatcherResults)}";
    }

    private static string AsyncModified(IMethodSymbol methodSymbol)
    {
        return methodSymbol.IsAsync() ? "async" : "";
    }

    private string GenerateMethodReturnType(IMethodSymbol methodSymbol)
    {
        return methodSymbol.ReturnType.ToDisplayString();
    }

    private string GetMethodConstraint(IMethodSymbol methodSymbol)
    {
        return methodSymbol.TypeParameters.Any() ? $"<{string.Join(", ", methodSymbol.TypeParameters)}>" : "";
    }

    private static string GenerateMethodParameters(IMethodSymbol methodSymbol)
    {
        return string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type} {p.Name}"));
    }
    
    private List<INamedTypeSymbol> GetInputImplementations()
    {
        var allTypes = new List<INamedTypeSymbol>();

        List<IAssemblySymbol> assemblySymbol = _context.Compilation.SourceModule.ReferencedAssemblySymbols.ToList();
        assemblySymbol.Add(_context.Compilation.Assembly);

        foreach (var assembly in assemblySymbol)
        {
            ProcessNamespace(assembly.GlobalNamespace, allTypes);
        }

        return allTypes;
    }

    private void ProcessNamespace(INamespaceSymbol namespaceSymbol, List<INamedTypeSymbol> allTypes)
    {
        foreach (var typeSymbol in namespaceSymbol.GetTypeMembers())
        {
            var implementsInputInterface = typeSymbol.AllInterfaces.Any(x =>
                SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, _pipelineConfig.InputType.ConstructedFrom));
            if (implementsInputInterface)
            {
                allTypes.Add(typeSymbol);
            }
        }

        foreach (var nestedNamespace in namespaceSymbol.GetNamespaceMembers())
        {
            ProcessNamespace(nestedNamespace, allTypes);
        }
    }

    // <Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>
    private static string GenerateGenericBrackets(bool hasResponse, ISymbol inputClass, List<ITypeSymbol> results,
        List<ITypeSymbol> inputResults)
    {
        if (!hasResponse)
        {
            return $"<{inputClass}>";
        }

        var builder = new StringBuilder();

        builder.Append($"<{inputClass}");

        // If input has generic response defined, then we need to use it, otherwise, use dispatcher results.
        if (inputResults.Count > 0)
        {
            foreach (var response in inputResults)
            {
                builder.Append($", {response}");
            }
        }
        else
        {
            foreach (var response in results)
            {
                builder.Append($", {response}");
            }
        }

        builder.Append(">");

        return builder.ToString();
    }

    // Example: return ((result.Item1 as TResult), (result.Item2 as TResult2));
    private string GenerateMultipleArgumentReturn(List<ITypeSymbol> results)
    {
        var builder = new StringBuilder();

        builder.Append("return (ValueTuple<");
        for (var index = 0; index < results.Count; index++)
        {
            var result = results[index];
            if (index > 0)
            {
                builder.Append(", ");
            }

            builder.Append(result);
        }

        builder.Append(">)result;");

        return builder.ToString();
    }

    // Example: return resultPipelinesTestsUseCasesHandlerWithResultSampleExampleCommand as TResult;
    private string GenerateSingleArgumentReturn(List<ITypeSymbol> handlerResults)
    {
        var builder = new StringBuilder();

        builder.Append($"return (");
        builder.Append(handlerResults.First());
        builder.Append(")");
        builder.Append($"result");
        builder.Append(";");

        return builder.ToString();
    }

    private string GenerateSingleCastExpression()
    {
        return _pipelineConfig.HandlerType.TypeArguments.Skip(1).First().Name;
    }

    private string GetConstraints(IMethodSymbol methodSymbol)
    {
        var typeParameterConstraints = methodSymbol.GetTypeParametersConstraints();
        if (typeParameterConstraints.Any())
        {
            return "where " + string.Join(" where ", typeParameterConstraints);
        }

        return "";
    }


    private string GetParametersString(IMethodSymbol method, int skip)
    {
        var parameters = string.Join(", ", method.Parameters.Skip(skip).Select(x => x.Name));
        return string.IsNullOrEmpty(parameters) ? "" : ", " + parameters;
    }

    static string GenerateCommasForGenericParameters(int n)
    {
        if (n <= 0)
        {
            return "";
        }
        
        return string.Join("", Enumerable.Repeat(",", n));
    }

    private void AddEmptyLine()
    {
        _builder.Append("\n");
    }

    private void AddLine(params string[] value)
    {
        _builder.Append("\n");
        _builder.Append(string.Join(" ", value));
    }

    private void AddInLine(params string[] value)
    {
        _builder.Append(string.Join("", value));
    }

    private void AddInLine(bool shouldAdd, params string[] value)
    {
        if (shouldAdd)
        {
            _builder.Append(string.Join("", value));
        }
    }

    private void AddInLine(bool shouldAdd, Func<string> builder)
    {
        if (shouldAdd)
        {
            _builder.Append(string.Join("", builder.Invoke()));
        }
    }
}