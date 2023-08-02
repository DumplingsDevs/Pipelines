using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable RS1035 // Do not use banned APIs for analyzers

namespace PipelinesGenerators;

[Generator]
public class DispatcherGenerator : ISourceGenerator
{
    private const string DispatcherAttribute = "PipelinesDispatcher";
    private const string ConfigInterfaceName = "IPipelineGeneratorConfig";

    private const string DispatcherTypeProperty = "GetDispatcherType";
    private const string InputTypeProperty = "GetInputType";
    private const string HandlerTypeProperty = "GetHandlerType";

    private const string CasePattern =
        "            case {0} r: return (await _serviceProvider.GetRequiredService<{2}<{0}, {1}>>().HandleAsync(r, token)) as TResult;";

    public void Execute(GeneratorExecutionContext context)
    {
        var caseBuilder = new StringBuilder();
        var configs = GetPipelineConfigs(context);

        foreach (var config in configs)
        {
            // Pobierz nazwę interfejsu
            var interfaceName = config.dispatcherType.Name;

            foreach (var classDeclaration in context.GetTypeNodes())
            {
                var semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                var interfaces = ((ITypeSymbol)classSymbol).AllInterfaces.ToList();
                var implementedInputInterface = interfaces.FirstOrDefault(x =>
                    SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, config.inputType));

                if (implementedInputInterface != null)
                {
                    var response = implementedInputInterface.TypeArguments.First();
                    caseBuilder.Append(string.Format(CasePattern, classSymbol.Name, response.Name,
                        config.handlerType.Name));
                    caseBuilder.Append("\n");
                }
            }

            // Wygeneruj kod klasy implementującej interfejs
            var classSourceCode = $@"
using System;
using Pipelines.Benchmarks.Types;
using Pipelines.Benchmarks.Sample;
using Microsoft.Extensions.DependencyInjection;

public class {interfaceName}Implementation : {interfaceName}
{{
    private readonly IServiceProvider _serviceProvider;

    public {interfaceName}Implementation(IServiceProvider serviceProvider)
    {{
        _serviceProvider = serviceProvider;
    }}

    {GenerateMethodImplementations((INamedTypeSymbol)config.dispatcherType, caseBuilder)}
}}";

            // Dodaj kod klasy do generacji
            var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
            var sourceFileName = $"{interfaceName}Implementation.cs";
            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static IEnumerable<(ITypeSymbol? dispatcherType, ITypeSymbol? inputType, ITypeSymbol? handlerType)>
        GetPipelineConfigs(GeneratorExecutionContext context)
    {
        var classes = context.GetClassNodeByInterface(ConfigInterfaceName).ToList();

        foreach (var classDeclarationSyntax in classes)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            var dispatcherType =
                classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, DispatcherTypeProperty);
            var inputType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, InputTypeProperty);
            var handlerType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, HandlerTypeProperty);

            // TO DO - throw exception when null/empty?
            yield return (dispatcherType, inputType, handlerType);
        }
    }

    private string GenerateMethodImplementations(INamedTypeSymbol interfaceSymbol, StringBuilder caseBuilder)
    {
        var methodImplementations = new StringBuilder();

        foreach (var methodSymbol in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            methodImplementations.AppendLine($@"
        public async {GenerateMethodReturnType(methodSymbol)} {methodSymbol.Name}{GetMethodConstraint(methodSymbol)}({GenerateMethodParameters(methodSymbol.Parameters)}){GetConstraints(methodSymbol)}
    {{
        switch (request)
        {{
{caseBuilder}
        }}

        throw new Exception();
    }}
                ");
        }

        return methodImplementations.ToString();
    }

    private string GenerateMethodParameters(ImmutableArray<IParameterSymbol> parameters)
    {
        return string.Join(", ", parameters.Select(p => $"{p.Type} {p.Name}"));
    }

    private string GetMethodConstraint(IMethodSymbol methodSymbol)
    {
        return methodSymbol.TypeParameters.Any() ? $"<{string.Join(", ", methodSymbol.TypeParameters)}>" : "";
    }

    private string GetConstraints(IMethodSymbol methodSymbol)
    {
        var typeParameterConstraints = methodSymbol.TypeParameters.Select(GetTypeParameterConstraints);
        if (typeParameterConstraints.Any())
        {
            return " where " + string.Join(" ", typeParameterConstraints);
        }

        return "";
    }

    private string GenerateMethodReturnType(IMethodSymbol methodSymbol)
    {
        return methodSymbol.ReturnType.ToDisplayString();
    }

    private string GetTypeParameterConstraints(ITypeParameterSymbol typeParameter)
    {
        var constraints = typeParameter.ConstraintTypes.Select(constraint => constraint.ToDisplayString());
        if (typeParameter.HasReferenceTypeConstraint)
        {
            constraints = constraints.Append("class");
        }

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

        return $"{typeParameter.Name} : {string.Join(", ", constraints)}";
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}