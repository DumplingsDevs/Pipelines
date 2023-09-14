using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Pipelines.WrapperDispatcherGenerator.Extensions;
using Pipelines.WrapperDispatcherGenerator.Validators.Dispatcher;
using Pipelines.WrapperDispatcherGenerator.Models;
using Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation;

namespace Pipelines.WrapperDispatcherGenerator.Builders;

internal class DispatcherImplementationBuilder
{
    private readonly StringBuilder _builder = new();
    private readonly PipelineConfig _pipelineConfig;
    private readonly GeneratorExecutionContext _context;

    private readonly IMethodSymbol _dispatcherMethod;
    private readonly IMethodSymbol _handlerMethod;
    private readonly string _dispatcherInterfaceName;

    public DispatcherImplementationBuilder(PipelineConfig pipelineConfig, GeneratorExecutionContext context)
    {
        _pipelineConfig = pipelineConfig;
        _context = context;

        CrossValidateResultTypes.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);
        CrossValidateParameters.Validate(_pipelineConfig.DispatcherType, _pipelineConfig.HandlerType);

        _dispatcherMethod = _pipelineConfig.DispatcherType.GetMembers().OfType<IMethodSymbol>().First();
        _handlerMethod = _pipelineConfig.HandlerType.ConstructedFrom.GetMembers().OfType<IMethodSymbol>().First();
        _dispatcherInterfaceName = _pipelineConfig.DispatcherType.GetFormattedFullname();

        CrossValidateGenericParameters.Validate(_dispatcherMethod, _pipelineConfig.HandlerType,
            _pipelineConfig.InputType);
    }

    public string Build()
    {
        BuildNamespaces();
        //TO DO - add info that file is generated (good practise)
        BuildClassDefinition();
        AddLine("{");
        BuildRequestHandlerBase();
        BuildRequestHandlerWrapper();
        AddLine("   private readonly IServiceProvider _serviceProvider;");
        AddLine("   private readonly DispatcherOptions _dispatcherOptions;");
        AddLine(
            $"   private readonly Dictionary<Type, {_dispatcherInterfaceName}RequestHandlerBase> _handlers = new();");
        BuildConstructor();
        BuildDispatcherHandlerMethod(_dispatcherMethod);
        AddLine("}");

        return _builder.ToString();
    }

    private void BuildNamespaces()
    {
        AddLine("using System;");
        AddLine("using System.Linq;");
        AddLine("using System.Collections.Generic;");
        AddLine("using System.Threading.Tasks;");
        AddLine("using Pipelines.Builder.HandlerWrappers;");
        AddLine("using Microsoft.Extensions.DependencyInjection;");
        AddLine("using Pipelines.Exceptions;");
        AddLine("using Pipelines;");
        AddLine("using Pipelines.Builder.Options;");
    }

    private void BuildRequestHandlerBase()
    {
        var methodParameters = GetDispatcherMethodParametersTypeName();
        var comma = string.IsNullOrEmpty(methodParameters) ? "" : ", ";
        var dispatcherResults = _dispatcherMethod.GetMethodResults();
        var hasResponse = dispatcherResults.Count > 0;
        string wrapperReturnType;
        if (hasResponse)
        {
            wrapperReturnType = _handlerMethod.IsAsync() ? "Task<object>" : "object";
        }
        else
        {
            wrapperReturnType = _handlerMethod.IsAsync() ? "Task" : "void";
        }

        AddLine(@$"
    private abstract class {_dispatcherInterfaceName}RequestHandlerBase
    {{
        internal abstract {wrapperReturnType} Handle(object request, {methodParameters}{comma} IServiceProvider serviceProvider, DispatcherOptions dispatcherOptions);
    }}");
    }

    private void BuildRequestHandlerWrapper()
    {
        var methodGenericResults = _handlerMethod.GetMethodGenericResults();
        var bracket = GenerateGenericParameters(methodGenericResults);
        var dispatcherResults = _dispatcherMethod.GetMethodResults();
        var hasResponse = dispatcherResults.Count > 0;
        var handlerReturnType = _handlerMethod.ReturnType;
        var handlerTypeName = _pipelineConfig.HandlerType.GetNameWithNamespace();
        var methodParameters = GetDispatcherMethodParametersTypeName();
        var wrapperMethodDefinitionComma = string.IsNullOrEmpty(methodParameters) ? "" : ", ";
        var parameterNames = GetParametersString(_dispatcherMethod, 1);
        var handlerCallComma = string.IsNullOrEmpty(parameterNames) ? "" : ", ";
        var inputInterfaceWithDispatchersGeneric = _pipelineConfig.InputType.ConstructedFrom.IsGenericType
            ? _pipelineConfig.InputType.GetNameWithNamespace() + $"<{string.Join(", ", methodGenericResults)}>"
            : _pipelineConfig.InputType.GetNameWithNamespace();
        var constrains =
            GenerateWrapperConstraints(inputInterfaceWithDispatchersGeneric, GetConstraints(_dispatcherMethod));
        var awaitOperator = _handlerMethod.IsAsync() ? "await" : "";
        var asyncModifier = _handlerMethod.IsAsync() ? "async" : "";
        var configureAwait = _handlerMethod.IsAsync() ? ".ConfigureAwait(false)" : "";
        string wrapperReturnType;
        if (hasResponse)
        {
            wrapperReturnType = _handlerMethod.IsAsync() ? "async Task<object>" : "object";
        }
        else
        {
            wrapperReturnType = _handlerMethod.IsAsync() ? "async Task" : "void";
        }

        var returnStatement = hasResponse ? "return" : "";
        var handleBody = hasResponse
            ? HandlerCallWithResult(handlerTypeName, bracket, returnStatement, awaitOperator, handlerCallComma,
                parameterNames)
            : HandlerCallWithoutResult(handlerTypeName, bracket, awaitOperator, handlerCallComma,
                parameterNames);


        AddLine(@$"
    private class {_dispatcherInterfaceName}RequestHandlerWrapperImpl{bracket} : {_dispatcherInterfaceName}RequestHandlerBase
        {constrains}
        
    {{
        internal override {wrapperReturnType} Handle(object request, {methodParameters}{wrapperMethodDefinitionComma} IServiceProvider serviceProvider, DispatcherOptions dispatcherOptions) =>
            {awaitOperator} Handle(({inputInterfaceWithDispatchersGeneric})request, serviceProvider, dispatcherOptions{handlerCallComma} {parameterNames}){configureAwait};

        private {asyncModifier} {handlerReturnType} Handle({inputInterfaceWithDispatchersGeneric} request, IServiceProvider serviceProvider, DispatcherOptions dispatcherOptions{wrapperMethodDefinitionComma}
            {methodParameters})
        {{
            {handleBody}
        }}
    }}");
    }

    private string GenerateWrapperConstraints(string inputInterfaceWithDispatchersGeneric, string constraints)
    {
        var constrains =
            _pipelineConfig.HandlerType.GetTypeParametersConstraints();
        var genericTypes = _pipelineConfig.HandlerType.TypeParameters.Select(x => x.Name).ToList();
        var inputConstraint = $"where TRequest : {constrains[0].Item2}";
        if (constrains.Count == genericTypes.Count)
        {
            for (int i = 1; i < genericTypes.Count; i++)
            {
                inputConstraint += $" where {genericTypes[i]} : {constrains[i].Item2}";
            }
        }

        return inputConstraint;
    }

    private string HandlerCallWithResult(string handlerTypeName, string handlerGenericParameters,
        string returnStatement, string awaitOperator, string handlerCallComma, string parameterNames)
    {
        return @$"
            if (dispatcherOptions.CreateDIScope)
            {{
                using var scope = serviceProvider.CreateScope();
                var provider = scope.ServiceProvider;

                var handler = provider.GetService<{handlerTypeName}{handlerGenericParameters}>();

                {returnStatement} {awaitOperator} handler.{_handlerMethod.Name}((TRequest)request{handlerCallComma} {parameterNames});
            }}
            else
            {{
                var handler = serviceProvider.GetService<{handlerTypeName}{handlerGenericParameters}>();

                {returnStatement} {awaitOperator} handler.{_handlerMethod.Name}((TRequest)request{handlerCallComma} {parameterNames});
            }}";
    }

    private string HandlerCallWithoutResult(string handlerTypeName, string handlerGenericParameters,
        string awaitOperator, string handlerCallComma, string parameterNames)
    {
        return @$"
            using var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;

            var handlers = provider.GetServices<{handlerTypeName}{handlerGenericParameters}>().ToList();
            if (handlers.Count == 0) throw new HandlerNotRegisteredException(typeof({handlerTypeName}{handlerGenericParameters}));
            foreach (var handler in handlers)
                {{
                    {awaitOperator} handler.{_handlerMethod.Name}((TRequest)request{handlerCallComma} {parameterNames});;
                }}";
    }


    private string GetDispatcherMethodParametersTypeName()
    {
        return string.Join(", ", _dispatcherMethod.Parameters.Skip(1).Select(p => $"{p.Type} @{p.Name}"));
    }

    private void BuildClassDefinition()
    {
        AddLine($"public class {_dispatcherInterfaceName}Implementation : {_pipelineConfig.DispatcherType}");
    }

    private void BuildConstructor()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();
        var methodGenericResults = _dispatcherMethod.GetMethodGenericResults();
        var wrapperGenericString = GenerateCommasForGenericParameters(methodGenericResults.Count);

        AddLine($@"
    public {dispatcherInterface}Implementation(IServiceProvider serviceProvider,
        IEnumerable<IHandlersRepository> handlerRepositories, IEnumerable<DispatcherGeneratorOptions> generatorDispatcherOptions)
    {{
        _serviceProvider = serviceProvider;
        var handlerTypes = handlerRepositories.First(x => x.DispatcherType == typeof({_pipelineConfig.DispatcherType})).GetHandlers();
        _dispatcherOptions = generatorDispatcherOptions.First(x => x.DispatcherType == typeof({_pipelineConfig.DispatcherType})).Options;

        foreach (var handlerType in handlerTypes)
        {{
            var handlers = handlerType.GetInterfaces()
                .Where(i => i.GetGenericTypeDefinition() == typeof({_pipelineConfig.HandlerType.GetNameWithNamespace()}<{wrapperGenericString}>))
                .ToList();
            foreach (var handler in handlers)
            {{
                var genericArguments = handler.GetGenericArguments();
                var requestType = genericArguments[0];
                var wrapperType = typeof({_dispatcherInterfaceName}RequestHandlerWrapperImpl<{wrapperGenericString}>).MakeGenericType(genericArguments);
                var wrapper = Activator.CreateInstance(wrapperType) ??
                              throw new CannotCreateDispatcherWrapperException(requestType);
                _handlers[requestType] = ({_dispatcherInterfaceName}RequestHandlerBase)wrapper;
            }}
        }}
    }}
");
    }

    private void BuildDispatcherHandlerMethod(IMethodSymbol dispatcherHandlerMethod)
    {
        var handlerMethod = _pipelineConfig.HandlerType.ConstructedFrom.GetMembers().OfType<IMethodSymbol>()
            .First();
        var dispatcherResults = dispatcherHandlerMethod.GetMethodResults();
        var hasMultipleResults = dispatcherResults.Count > 1;
        var hasResponse = dispatcherResults.Count > 0;
        var asyncModifier = handlerMethod.IsAsync() ? "await" : "";
        var inputParameterName = $"@{_dispatcherMethod.Parameters.First().Name}";

        AddLine("public",
            AsyncModified(dispatcherHandlerMethod),
            GenerateMethodReturnType(dispatcherHandlerMethod),
            dispatcherHandlerMethod.Name,
            GetMethodConstraint(dispatcherHandlerMethod),
            $"({GenerateMethodParameters(dispatcherHandlerMethod)})",
            GetConstraints(dispatcherHandlerMethod));

        AddLine("{");
        AddLine(@$"
        if ({inputParameterName} is null)
        {{
            throw new InputNullReferenceException();
        }}");
        AddLine($"var requestType = {inputParameterName}.GetType();");
        AddLine("if (!_handlers.TryGetValue(requestType, out var handlerWrapper))");
        AddLine("{");
        AddLine("throw new HandlerNotRegisteredException(requestType);");
        AddLine("}");
        AddLine(CallHandlerWrapperTemplate(hasResponse, hasMultipleResults, asyncModifier, dispatcherHandlerMethod,
            dispatcherResults));
        AddLine("}");
    }

    public string CallHandlerWrapperTemplate(bool hasResponse, bool hasMultipleResults, string @await,
        IMethodSymbol dispatcherMethod, List<ITypeSymbol> dispatcherResults)
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

    private string NoneResponseHandlerWrapper(string @await, IMethodSymbol dispatcherMethod,
        List<ITypeSymbol> dispatcherResults)
    {
        return $@"{@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider, _dispatcherOptions);";
    }

    private string MultipleResponsesHandlerWrapper(string @await, IMethodSymbol dispatcherMethod,
        List<ITypeSymbol> dispatcherResults)
    {
        return
            $@"var result = {@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider, _dispatcherOptions);
{GenerateMultipleArgumentReturn(dispatcherResults)};";
    }

    private string SingleResponseHandlerWrapper(string @await, IMethodSymbol dispatcherMethod,
        List<ITypeSymbol> dispatcherResults)
    {
        return
            $@"var result = {@await} handlerWrapper.Handle({GetParametersString(dispatcherMethod, 0)}, _serviceProvider, _dispatcherOptions);
{GenerateSingleArgumentReturn(dispatcherResults)};";
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
        return string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type} @{p.Name}"));
    }

    // <Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>
    private static string GenerateGenericParameters(List<ITypeSymbol> results)
    {
        if (results.Count == 0)
        {
            return $"<TRequest>";
        }

        var builder = new StringBuilder();

        builder.Append($"<TRequest");

        foreach (var response in results)
        {
            builder.Append($", {response}");
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
        return string.Join(", ", method.Parameters.Skip(skip).Select(x => $"@{x.Name}"));
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