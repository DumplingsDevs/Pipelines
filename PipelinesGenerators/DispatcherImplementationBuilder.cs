using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace PipelinesGenerators;

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
            AsyncModified(),
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
        AddLine("switch", $"({methodSymbol.Parameters.First().Name})");
        AddLine("{");
        BuildSwitchBody();
        AddLine("}");
        AddLine("throw new Exception();"); //TO DO - dedicated exception?
    }

    private void BuildSwitchBody()
    {
        foreach (var classDeclaration in _context.GetTypeNodes())
        {
            var semanticModel = _context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            var interfaces = ((ITypeSymbol)classSymbol).AllInterfaces.ToList();
            var implementedInputInterface = interfaces.FirstOrDefault(x =>
                SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, _pipelineConfig.InputType));

            if (implementedInputInterface != null)
            {
                var response = implementedInputInterface.TypeArguments.First();
                var handlerMethodName = _pipelineConfig.HandlerType.GetMembers().First().Name;
                
                AddLine("case",
                    classSymbol.Name,
                    "r: return",
                    "(",
                    "await",
                    $"_serviceProvider.GetRequiredService<{_pipelineConfig.HandlerType.Name}<{classSymbol.Name}, {response.Name}>>().{handlerMethodName}(r, token)) as TResult;");
            }
        }
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
            return " where " + string.Join(" ", typeParameterConstraints);
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

    private string AsyncModified()
    {
        if (true)
        {
            return "async";
        }
    }

    private void BuildConstructor()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.Name;

        AddLine($"public {dispatcherInterface}Implementation(IServiceProvider serviceProvider)");
        AddLine("{");
        AddLine("_serviceProvider = serviceProvider;");
        AddLine("}");
    }

    private void BuildClassDefinition()
    {
        var dispatcherInterface = _pipelineConfig.DispatcherType.Name;

        //TO DO: What if someone will make the same name of dispatcher, but in different namespaces?
        AddLine($"public class {dispatcherInterface}Implementation : {dispatcherInterface}");
    }

    private void BuildNamespaces()
    {
        AddLine("using System;");
        AddLine("using Pipelines.Benchmarks.Types;");
        AddLine("using Pipelines.Benchmarks.Sample;");
        AddLine("using Microsoft.Extensions.DependencyInjection;");
    }

    private void AddLine(params string[] value)
    {
        _builder.Append(string.Join(" ", value) + "\n");
    }
}