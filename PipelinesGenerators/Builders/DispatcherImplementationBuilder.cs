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
        //TO DO: throw dedicated exception
        AddLine("default: throw new Exception();");
        AddLine("}");
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
                //implementedInputInterface.TypeArguments.Count > 1 then Tuple scenario
                var response = implementedInputInterface.TypeArguments.FirstOrDefault();
                var hasResponse = response is not null;
                var handlerMethodName = _pipelineConfig.HandlerType.GetMembers().First().Name;
                var genericStructure = hasResponse ? $"<{classSymbol}, {response}>" : $"<{classSymbol}>";
                var isAsync = true;
                var resultName = $"result{classSymbol.GetFormattedFullname()}";

                AddInLine("case ");
                AddInLine(classSymbol.ToString());
                AddInLine(" r: ");
                AddEmptyLine();
                AddInLine(hasResponse, $"var {resultName} = ");
                AddInLine(isAsync, "await ");
                AddInLine("_serviceProvider.GetRequiredService<");
                AddInLine(_pipelineConfig.HandlerType.GetNameWithNamespace());
                AddInLine(genericStructure);
                AddInLine(">().", handlerMethodName, "(r, token);");
                AddEmptyLine();
                AddInLine(hasResponse, $"return {resultName} ");
                AddInLine(hasResponse, "as TResult;");
                AddEmptyLine();
                AddInLine(!hasResponse, "break;");
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
        // check if in body exists await 
        if (true)
        {
            return "async";
        }
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
}