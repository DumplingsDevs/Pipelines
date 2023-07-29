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
    private const string AttributeName = "GenerateImplementation"; // Atrybut oznaczający interfejsy do wygenerowania

    public void Execute(GeneratorExecutionContext context)
    {
        // Pobierz wszystkie interfejsy z atrybutem GenerateImplementation
        var interfacesToImplement = context.Compilation.SyntaxTrees
            .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>())
            .Where(interfaceSyntax => interfaceSyntax.AttributeLists
                .SelectMany(list => list.Attributes)
                .Any(attribute => attribute.Name.ToString() == AttributeName))
            .ToList();

        foreach (var interfaceSyntax in interfacesToImplement)
        {
            // Pobierz nazwę interfejsu
            var interfaceName = interfaceSyntax.Identifier.ValueText;

            // Wygeneruj kod klasy implementującej interfejs
            var classSourceCode = $@"
using System;
using Pipelines.Benchmarks.Types;
using Microsoft.Extensions.DependencyInjection;

public class {interfaceName}Implementation : {interfaceName}
{{
    private readonly IServiceProvider _serviceProvider;

    public {interfaceName}Implementation(IServiceProvider serviceProvider)
    {{
        _serviceProvider = serviceProvider;
    }}

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
    {{
            var type = request.GetType();
            var handlerGeneric = typeof(IRequestHandler<,>);
            var requestAndResult = new[] {{ type, typeof(TResult) }};
            var handlerType = handlerGeneric.MakeGenericType(requestAndResult);

            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            return (TResult)await handler.HandleAsync((dynamic)request, token);
    }}
}}";

            // Dodaj kod klasy do generacji
            var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
            var sourceFileName = $"{interfaceName}Implementation.cs";
            context.AddSource(sourceFileName, sourceText);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}