using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Pipelines.Generators.Builders;
using Pipelines.Generators.Models;
using Pipelines.Generators.Extensions;

#pragma warning disable RS1035 // Do not use banned APIs for analyzers

namespace Pipelines.Generators;

[Generator]
public class DispatcherGenerator : ISourceGenerator
{
    private const string ConfigInterfaceName = "IPipelineGeneratorConfig";

    private const string DispatcherTypeProperty = "GetDispatcherType";
    private const string InputTypeProperty = "GetInputType";
    private const string HandlerTypeProperty = "GetHandlerType";
    
    public void Execute(GeneratorExecutionContext context)
    {
        var configs = GetPipelineConfigs(context);

        foreach (var config in configs)
        {
            var interfaceName = config.DispatcherType.GetNameWithNamespace();

            var builder = new DispatcherImplementationBuilder(config, context);
            var classSourceCode = builder.Build();
            var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
            var sourceFileName = $"{interfaceName}Implementation.g.cs";
            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static IEnumerable<PipelineConfig> GetPipelineConfigs(GeneratorExecutionContext context)
    {
        var classes = context.GetClassNodeByInterface(ConfigInterfaceName).ToList();

        foreach (var classDeclarationSyntax in classes)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            var dispatcherType =
                classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, DispatcherTypeProperty);
            var inputType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, InputTypeProperty);
            var handlerType = classDeclarationSyntax.GetPropertyTypeSymbol(semanticModel, HandlerTypeProperty);

            if (dispatcherType is null || handlerType is null || inputType is null)
            {
                // TO DO - throw exception when null/empty?
                continue;
            }
            
            yield return new PipelineConfig(dispatcherType, handlerType, inputType);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}