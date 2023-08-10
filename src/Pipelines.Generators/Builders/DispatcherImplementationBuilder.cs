using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Models;
using Pipelines.Generators.Extensions;

namespace Pipelines.Generators.Builders;

internal class DispatcherImplementationBuilder
{
    private readonly StringBuilder _builder = new();
    private readonly PipelineConfig _pipelineConfig;
    private readonly GeneratorExecutionContext _context;

    public DispatcherImplementationBuilder(PipelineConfig pipelineConfig, GeneratorExecutionContext context)
    {
        _pipelineConfig = pipelineConfig;
        _context = context;
    }

    public string Build()
    {
        BuildNamespaces();
        //TO DO - add info that file is generated (good practise)
        BuildClassDefinition();
        AddLine("{");
        AddLine("private readonly IServiceProvider _serviceProvider;");
        BuildConstructor();
        BuildMethodImplementations();
        AddLine("}");

        return _builder.ToString();
    }

    private void BuildNamespaces()
    {
        AddLine("using System;");
        AddLine("using Microsoft.Extensions.DependencyInjection;");
        AddLine("using Pipelines.Exceptions;");
    }

    private void BuildClassDefinition()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();

        AddLine($"public class {dispatcherInterface}Implementation : {_pipelineConfig.DispatcherType}");
    }

    private void BuildConstructor()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();

        AddLine($"public {dispatcherInterface}Implementation(IServiceProvider serviceProvider)");
        AddLine("{");
        AddLine("_serviceProvider = serviceProvider;");
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
        BuildSwitchCase(methodSymbol);
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
            var handlerResults = GetHandlerArgumentResults(handlerMethod);
            var hasMultipleResults = handlerResults.Count > 1;
            var inputResults = implementedInputInterface.TypeArguments.ToList();
            var hasResponse = handlerResults.Count > 0;
            var dispatcherMethod = _pipelineConfig.DispatcherType.GetMembers().OfType<IMethodSymbol>().First();
            var genericStructure = GenerateGenericBrackets(hasResponse, inputClass, inputResults);
            var asyncModifier = handlerMethod.IsAsync() ? "await" : "";
            var resultName = $"result{inputClass.GetFormattedFullname()}";

            AddInLine("case ");
            AddInLine(inputClass.ToString());
            AddInLine(" i: ");
            AddLine(GetCaseTemplate(hasResponse,
                hasMultipleResults,
                resultName,
                asyncModifier,
                $"{_pipelineConfig.HandlerType.GetNameWithNamespace()}{genericStructure}",
                handlerMethod,
                dispatcherMethod,
                handlerResults));
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
        var handlerResultTypeArguments = ((INamedTypeSymbol)methodSymbol.ReturnType).TypeArguments.ToList();

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
            return @$"var {resultName}Handler = _serviceProvider.GetService<{handlerType}>();
            if ({resultName}Handler is null) throw new HandlerNotRegisteredException(typeof({handlerType}));
            var {resultName} = {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
            {GenerateSingleArgumentReturn(resultName, handlerResults)}";
        }

        if (hasResponse && hasMultipleResults)
        {
            return @$"var {resultName}Handler = _serviceProvider.GetService<{handlerType}>();
            if ({resultName}Handler is null) throw new HandlerNotRegisteredException(typeof({handlerType}));
            var {resultName} = {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
            {GenerateMultipleArgumentReturn(resultName, handlerResults)}";
        }

        if (!hasResponse)
        {
            return @$"var {resultName}Handlers = _serviceProvider.GetServices<{handlerType}>();
            foreach (var {resultName}Handler in {resultName}Handlers)
                {{
                    {@await} {resultName}Handler.{handlerMethod.Name}(i{GetParametersString(dispatcherMethod, 1)});
                }}
            break;";
        }

        return "";
    }

    // <Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>
    private static string GenerateGenericBrackets(bool hasResponse, ISymbol inputClass, List<ITypeSymbol> results)
    {
        if (!hasResponse)
        {
            return $"<{inputClass}>";
        }

        var builder = new StringBuilder();

        builder.Append($"<{inputClass}");
        foreach (var response in results)
        {
            builder.Append($", {response}");
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
        var typeParameterConstraints = methodSymbol.TypeParameters.Select(GetTypeParameterConstraints).ToList();
        if (typeParameterConstraints.Any())
        {
            return "where " + string.Join(" where ", typeParameterConstraints);
        }

        return "";
    }

    private string GetTypeParameterConstraints(ITypeParameterSymbol typeParameter)
    {
        var constraints = typeParameter.ConstraintTypes.Select(constraint => constraint.ToDisplayString());
        if (typeParameter.HasReferenceTypeConstraint)
        {
            constraints = constraints.Append("class");
        }

        if (typeParameter.HasValueTypeConstraint)
        {
            constraints = constraints.Append("struct");
        }

        if (typeParameter.HasConstructorConstraint)
        {
            constraints = constraints.Append("new()");
        }

        //TO DO - other constraints ? Or maybe different approach

        return $"{typeParameter.Name} : {string.Join(", ", constraints)}";
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