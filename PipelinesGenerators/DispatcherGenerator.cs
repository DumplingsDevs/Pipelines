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
    private const string ConfigInterfaceName = "IPipelineGeneratorConfig";
    
    private const string DispatcherTypeProperty = "GetDispatcherType";
    private const string InputTypeProperty = "GetInputType";
    private const string HandlerTypeProperty = "GetHandlerType";

    public void Execute(GeneratorExecutionContext context)
    {
        var configs = GetPipelineConfigs(context);

        // Pobierz wszystkie interfejsy z atrybutem GenerateImplementation
        var interfacesToImplement = GetInterfacesWithAttribute(context, DispatcherAttribute);

        foreach (var config in configs)
        {
            // Pobierz nazwę interfejsu
            var interfaceName = config.dispatcherType.Name;

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
        
        static IEnumerable<(ITypeSymbol? dispatcherType, ITypeSymbol? inputType, ITypeSymbol? handlerType)> GetPipelineConfigs(GeneratorExecutionContext context)
        {
            var classes = context.GetClassNodeByInterface(ConfigInterfaceName).ToList();
        
            foreach (var (classDeclarationSyntax, syntaxTree) in classes)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                var dispatcherType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, DispatcherTypeProperty);
                var inputType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, InputTypeProperty);
                var handlerType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, HandlerTypeProperty);
                
                // TO DO - throw exception when null/empty?
                yield return (dispatcherType, inputType, handlerType);
            }
        }
    }

    private static List<InterfaceDeclarationSyntax> GetInterfacesWithAttribute(GeneratorExecutionContext context,
        string disptacherAttribute)
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