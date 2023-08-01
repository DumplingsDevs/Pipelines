using System.Collections.Generic;
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

    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxTrees = context.Compilation.SyntaxTrees;

        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (root == null) continue;

            var pipelinesConfigClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.ValueText == "PipelinesConfig");
            
            if (pipelinesConfigClass != null)
            {
                var getDispatcherProperty = pipelinesConfigClass.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(p => p.Identifier.ValueText == "GetDispatcher");

                if ( getDispatcherProperty != null )
                {
                    var typeOfSyntax = getDispatcherProperty.DescendantNodes().OfType<TypeOfExpressionSyntax>().FirstOrDefault();
                    if ( typeOfSyntax != null )
                    {
                        var identifierSyntax = typeOfSyntax.ChildNodes().OfType<IdentifierNameSyntax>().FirstOrDefault();
                        if ( identifierSyntax != null )
                        {
                            var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                            var typeInfo = semanticModel.GetTypeInfo( identifierSyntax );
                            var symbolInfoSymbol = typeInfo.Type;
                            
                        }
                    }
                }
            }
        }

        // Pobierz wszystkie interfejsy z atrybutem GenerateImplementation
        var interfacesToImplement = GetInterfacesWithAttribute(context, DispatcherAttribute);

        foreach (var interfaceSyntax in interfacesToImplement)
        {
            // Pobierz nazwę interfejsu
            var interfaceName = interfaceSyntax.Identifier.ValueText;

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

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token) where TResult : class
    {{
        switch (request)
        {{
            case ExampleRequest r:
                var handler = _serviceProvider.GetRequiredService<IRequestHandler<ExampleRequest, ExampleCommandResult>>();
                return (await handler.HandleAsync(r, token)) as TResult;

        }}

        throw new Exception();
    }}
}}";

            // Dodaj kod klasy do generacji
            var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
            var sourceFileName = $"{interfaceName}Implementation.cs";
            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static List<InterfaceDeclarationSyntax> GetInterfacesWithAttribute(GeneratorExecutionContext context, string disptacherAttribute)
    {
        return context.Compilation.SyntaxTrees
            .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>())
            .Where(interfaceSyntax => interfaceSyntax.AttributeLists
                .SelectMany(list => list.Attributes)
                .Any(attribute => attribute.Name.ToString() == disptacherAttribute))
            .ToList();
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}