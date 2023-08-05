using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using PipelinesGenerators.Extensions;
using PipelinesGenerators.Models;

namespace PipelinesGenerators.Builders;

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

    private void BuildSwitchCase(IMethodSymbol methodSymbol)
    {
        var parameterName = methodSymbol.Parameters.First().Name;
        
        AddLine("switch", $"({parameterName})");
        AddLine("{");
        BuildSwitchBody();
        //TO DO: throw dedicated exception
        AddLine($"default: throw new InputNotSupportedByDispatcherException({parameterName}.GetType(), typeof({_pipelineConfig.DispatcherType}));");
        AddLine("}");
    }

    // Example:
    //     case Sample.ExampleCommand r:
    //     var result = _serviceProvider.GetRequiredService<ICommandHandler<Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>>().HandleAsync(r, token);
    //     return ((result.Item1 as TResult), (result.Item2 as TResult2));
    private void BuildSwitchBody()
    {
        foreach (var classDeclaration in _context.GetTypeNodes())
        {
            var semanticModel = _context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var inputClass = semanticModel.GetDeclaredSymbol(classDeclaration);
            var interfaces = ((ITypeSymbol)inputClass).AllInterfaces.ToList();
            var implementedInputInterface = interfaces.FirstOrDefault(x =>
                SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, _pipelineConfig.InputType));

            if (implementedInputInterface != null)
            {
                var hasMultipleResults = implementedInputInterface.TypeArguments.Length > 1;
                var genericResults = _pipelineConfig.HandlerType.TypeArguments.Skip(1).ToList();
                var results = implementedInputInterface.TypeArguments.ToList();
                var hasResponse = results.Count > 0;
                var handlerMethod = _pipelineConfig.HandlerType.GetMembers().OfType<IMethodSymbol>().First();
                var handlerMethodName = handlerMethod.Name;
                var genericStructure = GenerateGenericBrackets(hasResponse, inputClass, results);
                var isAsync = handlerMethod.IsAsync();
                var resultName = $"result{inputClass.GetFormattedFullname()}";

                // TO DO: Check if GetService returns null and throw dedicated exception
                AddInLine("case ");
                AddInLine(inputClass.ToString());
                AddInLine(" r: ");
                AddEmptyLine();
                AddInLine(hasResponse, $"var {resultName} = ");
                AddInLine(isAsync, "await ");
                AddInLine("_serviceProvider.GetRequiredService<");
                AddInLine(_pipelineConfig.HandlerType.GetNameWithNamespace());
                AddInLine(genericStructure);
                AddInLine(">().", handlerMethodName, "(r, token);");
                AddEmptyLine();
                AddInLine(hasResponse && !hasMultipleResults, () => GenerateSingleArgumentReturn(resultName));
                AddInLine(hasResponse && hasMultipleResults,
                    () => GenerateMultipleArgumentReturn(resultName, genericResults));
                AddEmptyLine();
                AddInLine(!hasResponse, "break;");
            }
        }
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
    private string GenerateSingleArgumentReturn(string resultName)
    {
        var builder = new StringBuilder();

        builder.Append($"return {resultName} ");
        builder.Append("as ");
        builder.Append(GenerateSingleCastExpression());
        builder.Append(";");

        return builder.ToString();
    }

    private string GenerateMultipleCastExpression()
    {
        throw new System.NotImplementedException();
    }

    private string GenerateSingleCastExpression()
    {
        return _pipelineConfig.HandlerType.TypeArguments.Skip(1).First().Name;
    }

    private string GenerateMethodParameters(IMethodSymbol methodSymbol)
    {
        return string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type} {p.Name}"));
    }

    private string GetMethodConstraint(IMethodSymbol methodSymbol)
    {
        return methodSymbol.TypeParameters.Any() ? $"<{string.Join(", ", methodSymbol.TypeParameters)}>" : "";
    }

    private string GetConstraints(IMethodSymbol methodSymbol)
    {
        var typeParameterConstraints = methodSymbol.TypeParameters.Select(GetTypeParameterConstraints).ToList();
        if (typeParameterConstraints.Any())
        {
            //TO DO - I think that many constraints should be in separate lines
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

        //TO DO
        // if (typeParameter.HasValueTypeConstraint && !typeParameter.HasValueTypeConstraintIsNullable)
        // {
        //     constraints = constraints.Append("struct");
        // }
        //
        // if (typeParameter.HasValueTypeConstraintIsNullable)
        // {
        //     constraints = constraints.Append("struct?");
        // }

        if (typeParameter.HasConstructorConstraint)
        {
            constraints = constraints.Append("new()");
        }

        //TO DO - other constraints ? Or maybe different approach

        return $"{typeParameter.Name} : {string.Join(", ", constraints)}";
    }

    private string GenerateMethodReturnType(IMethodSymbol methodSymbol)
    {
        return methodSymbol.ReturnType.ToDisplayString();
    }

    private string AsyncModified(IMethodSymbol methodSymbol)
    {
        // check if in body exists await 
        if (methodSymbol.IsAsync())
        {
            return "async";
        }

        return "";
    }

    private void BuildConstructor()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();

        AddLine($"public {dispatcherInterface}Implementation(IServiceProvider serviceProvider)");
        AddLine("{");
        AddLine("_serviceProvider = serviceProvider;");
        AddLine("}");
    }

    private void BuildClassDefinition()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.GetFormattedFullname();

        //TO DO: What if someone will make the same name of dispatcher, but in different namespaces?
        AddLine($"public class {dispatcherInterface}Implementation : {_pipelineConfig.DispatcherType}");
    }

    private void BuildNamespaces()
    {
        AddLine("using System;");
        AddLine("using Microsoft.Extensions.DependencyInjection;");
        AddLine("using Pipelines.Exceptions;");
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