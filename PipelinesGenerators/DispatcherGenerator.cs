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
        
        foreach (var config in configs)
        {
            // Pobierz nazwę interfejsu
            var interfaceName = config.dispatcherType.Name;

            foreach (var classDeclaration in context.GetTypeNodes())
            {
                var semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                var interfaces = ((ITypeSymbol)classSymbol).AllInterfaces.ToList();
                var constructedFrom = interfaces.FirstOrDefault()?.ConstructedFrom;

                if (SymbolEqualityComparer.Default.Equals(constructedFrom, config.inputType))
                {
                    
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

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token) where TResult : class
    {{
        switch (request)
        {{
            case ExampleRequest r: return (await _serviceProvider.GetRequiredService<IRequestHandler<ExampleRequest, ExampleCommandResult>>().HandleAsync(r, token)) as TResult;

        }}

        throw new Exception();
    }}
}}";

            // Dodaj kod klasy do generacji
            var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
            var sourceFileName = $"{interfaceName}Implementation.cs";
            context.AddSource(sourceFileName, sourceText);
        }

        static IEnumerable<(ITypeSymbol? dispatcherType, ITypeSymbol? inputType, ITypeSymbol? handlerType)>
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
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}