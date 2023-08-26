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

    public DispatcherBuilder(PipelineConfig pipelineConfig, GeneratorExecutionContext context)
    {
        _pipelineConfig = pipelineConfig;
        _context = context;

        DispatcherParameterConstraintValidator.Validate(_pipelineConfig.DispatcherType);
        CrossValidateResultTypes.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);
        CrossValidateParameters.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);
    }

    public string Build()
    {
        BuildNamespaces();
        //TO DO - add info that file is generated (good practise)
        BuildRequestHandlerBase();
        BuildClassDefinition();
        AddLine("{");
        AddLine("private readonly IServiceProvider _serviceProvider;");
        AddLine("private readonly Dictionary<Type, RequestHandlerBase> _handlers = new();");
        BuildConstructor();
        BuildMethodImplementations();
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
internal abstract class RequestHandlerBase
{
    internal abstract Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}");
    }

    private void BuildRequestHandlerWrapper()
    {
        AddLine(@"
internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerBase
    where TRequest : IRequest<TResponse> where TResponse : class
//TO DO: Probably TResponse don't need to be class constraint
{
    internal override async Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken) =>
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
        AddLine("var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(genericArguments);");
        AddLine(@"var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($""Could not create wrapper type for {requestType}"");");
        AddLine("_handlers[requestType] = (RequestHandlerBase)wrapper;");
        AddLine("}");
    }

    private void BuildMethodImplementations()
    {
        var dispatcherMethods = _pipelineConfig.DispatcherType.GetMembers().OfType<IMethodSymbol>();

        foreach (var methodSymbol in dispatcherMethods)
        {
            BuildMethod(methodSymbol);
        }
    }

    private void BuildMethod(IMethodSymbol methodSymbol)
    {
        AddLine("public",
            AsyncModified(methodSymbol),
            GenerateMethodReturnType(methodSymbol),
            methodSymbol.Name,
            GetMethodConstraint(methodSymbol),
            $"({GenerateMethodParameters(methodSymbol)})",
            GetConstraints(methodSymbol));

        AddLine("{");
        AddLine("var requestType = request.GetType();");
        AddLine("}");
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

    private void BuildSwitchCase(IMethodSymbol methodSymbol)
    {
        var parameterName = methodSymbol.Parameters.First().Name;

        AddLine("switch", $"({parameterName})");
        AddLine("{");
        BuildSwitchBody();
        AddLine(
            $"default: throw new InputNotSupportedByDispatcherException({parameterName}.GetType(), typeof({_pipelineConfig.DispatcherType}));");
        AddLine("}");
    }

    // Example:
    //     case Sample.ExampleCommand r:
    //     var result = _serviceProvider.GetRequiredService<ICommandHandler<Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>>().HandleAsync(r, token);
    //     return ((result.Item1 as TResult), (result.Item2 as TResult2));
    private void BuildSwitchBody()
    {
        var inputImplementations = GetInputImplementations();
        foreach (var inputClass in inputImplementations)
        {
            var interfaces = inputClass.AllInterfaces.ToList();
            var implementedInputInterface = interfaces.FirstOrDefault(x =>
                SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, _pipelineConfig.InputType.ConstructedFrom));

            var handlerMethod = _pipelineConfig.HandlerType.ConstructedFrom.GetMembers().OfType<IMethodSymbol>()
                .First();
            var dispatcherMethod = _pipelineConfig.DispatcherType.GetMembers().OfType<IMethodSymbol>().First();
            var dispatcherResults = GetHandlerArgumentResults(dispatcherMethod);
            var inputResults = implementedInputInterface.TypeArguments.ToList();
            var hasMultipleResults = dispatcherResults.Count > 1;
            var dispatcherArgumentResults = dispatcherMethod.TypeArguments.ToList();
            var hasResponse = dispatcherResults.Count > 0;
            var genericStructure =
                GenerateGenericBrackets(hasResponse, inputClass, dispatcherArgumentResults, inputResults);
            var asyncModifier = handlerMethod.IsAsync() ? "await" : "";
            var resultName = $"result{inputClass.GetFormattedFullname()}";

            AddLine("case ");
            AddInLine(inputClass.ToString());
            AddInLine(" i: ");
            AddLine(GetCaseTemplate(hasResponse,
                hasMultipleResults,
                resultName,
                asyncModifier,
                $"{_pipelineConfig.HandlerType.GetNameWithNamespace()}{genericStructure}",
                handlerMethod,
                dispatcherMethod,
                dispatcherResults));
        }
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

    private List<ITypeSymbol> GetHandlerArgumentResults(IMethodSymbol methodSymbol)
    {
        if (methodSymbol.ReturnsVoid)
        {
            return new List<ITypeSymbol>();
        }

        if (methodSymbol.ReturnType is ITypeParameterSymbol typeParameterSymbol)
        {
            return new List<ITypeSymbol>() { typeParameterSymbol };
        }

        var namedSymbol = ((INamedTypeSymbol)methodSymbol.ReturnType);

        if (namedSymbol.IsVoidTask())
        {
            return new List<ITypeSymbol>();
        }

        if (!namedSymbol.IsGenericType)
        {
            return new List<ITypeSymbol>() { namedSymbol };
        }

        var handlerResultTypeArguments = namedSymbol.TypeArguments.ToList();

        if (handlerResultTypeArguments.Count > 1)
        {
            return handlerResultTypeArguments;
        }

        if (handlerResultTypeArguments.Count == 0)
        {
            return new List<ITypeSymbol>();
        }

        switch (handlerResultTypeArguments.First())
        {
            case INamedTypeSymbol taskNamedResult:
            {
                var taskArguments = taskNamedResult.TypeArguments;

                return taskArguments.Length > 0 ? taskArguments.ToList() : new List<ITypeSymbol> { taskNamedResult };
            }
            case ITypeParameterSymbol taskTypeResult:
                return new List<ITypeSymbol> { taskTypeResult };
        }

        return new List<ITypeSymbol>();
    }

    private string GetCaseTemplate(bool hasResponse, bool hasMultipleResults, string resultName, string @await,
        string handlerType, IMethodSymbol handlerMethod, IMethodSymbol dispatcherMethod,
        List<ITypeSymbol> handlerResults)
    {
        if (hasResponse && !hasMultipleResults)
        {
            return CaseBodyForSingleResponse(resultName, @await, handlerType, handlerMethod, dispatcherMethod,
                handlerResults);
        }

        if (hasResponse && hasMultipleResults)
        {
            return CaseBodyForMultipleResponses(resultName, @await, handlerType, handlerMethod, dispatcherMethod,
                handlerResults);
        }

        if (!hasResponse)
        {
            return CaseBodyWithoutResult(resultName, @await, handlerType, handlerMethod, dispatcherMethod);
        }

        return "";
    }

    private string CaseBodyWithoutResult(string resultName, string await, string handlerType,
        IMethodSymbol handlerMethod,
        IMethodSymbol dispatcherMethod)
    {
        return @$"var {resultName}Handlers = _serviceProvider.GetServices<{handlerType}>().ToList();
            if ({resultName}Handlers.Count == 0) throw new HandlerNotRegisteredException(typeof({handlerType}));
            foreach (var {resultName}Handler in {resultName}Handlers)
                {{
                    {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
                }}
            break;";
    }

    private string CaseBodyForMultipleResponses(string resultName, string await, string handlerType,
        IMethodSymbol handlerMethod, IMethodSymbol dispatcherMethod, List<ITypeSymbol> handlerResults)
    {
        return @$"var {resultName}Handler = _serviceProvider.GetService<{handlerType}>();
            if ({resultName}Handler is null) throw new HandlerNotRegisteredException(typeof({handlerType}));
            var {resultName} = {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
            {GenerateMultipleArgumentReturn(resultName, handlerResults)}";
    }

    private string CaseBodyForSingleResponse(string resultName, string await, string handlerType,
        IMethodSymbol handlerMethod, IMethodSymbol dispatcherMethod, List<ITypeSymbol> handlerResults)
    {
        return @$"var {resultName}Handler = _serviceProvider.GetService<{handlerType}>();
            if ({resultName}Handler is null) throw new HandlerNotRegisteredException(typeof({handlerType}));
            var {resultName} = {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
            {GenerateSingleArgumentReturn(resultName, handlerResults)}";
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
    private string GenerateMultipleArgumentReturn(string resultName, List<ITypeSymbol> results)
    {
        var builder = new StringBuilder();

        builder.Append("return (");
        for (var index = 0; index < results.Count; index++)
        {
            var result = results[index];
            if (index > 0)
            {
                builder.Append(", ");
            }

            builder.Append($"({resultName}.Item{index + 1} as {result})");
        }

        builder.Append(");");

        return builder.ToString();
    }

    // Example: return resultPipelinesTestsUseCasesHandlerWithResultSampleExampleCommand as TResult;
    private string GenerateSingleArgumentReturn(string resultName, List<ITypeSymbol> handlerResults)
    {
        var builder = new StringBuilder();

        builder.Append($"return {resultName} ");
        builder.Append("as ");
        builder.Append(handlerResults.First());
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