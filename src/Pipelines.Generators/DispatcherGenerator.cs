using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        // GetPipelineConfigs2(context);
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

    private void GetPipelineConfigs2(GeneratorExecutionContext context)
    {
        var syntaxTrees = context.Compilation.SyntaxTrees;

        // Process all syntax trees in the compilation
        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetRoot();
            var syntaxReceiver = new SyntaxReceiver(context.Compilation);

            // Visit the syntax tree to find the relevant method invocations
            syntaxReceiver.Visit(root);

            // Check if we have found all necessary method invocations
            if (syntaxReceiver is
                { AddInputInvocation: not null, AddHandlerInvocation: not null, AddDispatcherInvocation: not null })
            {
                var inputType = GetTypeSymbol(context.Compilation, syntaxReceiver.AddInputInvocation);
                var handlerType = GetTypeSymbol(context.Compilation, syntaxReceiver.AddHandlerInvocation);
                var dispatcherType =
                    GetTypeSymbol(context.Compilation, syntaxReceiver.AddDispatcherInvocation);
                
                // yield return new PipelineConfig(dispatcherType, handlerType, inputType);
            }
        }
    }

    private ITypeSymbol GetTypeSymbol(Compilation compilation, InvocationExpressionSyntax invocationSyntax)
    {
        var childs = invocationSyntax.ChildNodes().ToList();
        var childs2 = invocationSyntax.DescendantNodes().ToList();
        var typeOfSyntax = invocationSyntax.DescendantNodes().OfType<TypeOfExpressionSyntax>()
            .FirstOrDefault();
        if (typeOfSyntax != null)
        {
            var semanticModel = compilation.GetSemanticModel(invocationSyntax.SyntaxTree);
            return ModelExtensions.GetSymbolInfo(semanticModel, invocationSyntax).Symbol as INamedTypeSymbol;    
        }

        return null;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
    
    private class SyntaxReceiver : CSharpSyntaxWalker
    {
        private readonly Compilation _compilation;

        public SyntaxReceiver(Compilation compilation, SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node) : base(depth)
        {
            _compilation = compilation;
        }

        public InvocationExpressionSyntax AddInputInvocation { get; private set; }
        public InvocationExpressionSyntax AddHandlerInvocation { get; private set; }
        public InvocationExpressionSyntax AddDispatcherInvocation { get; private set; }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // Visit each invocation expression and check if it matches our methods
            var methodSymbol = ModelExtensions.GetSymbolInfo(_compilation.GetSemanticModel(node.SyntaxTree), node).Symbol as IMethodSymbol;
            if (methodSymbol?.Name == "AddInput")
            {
                AddInputInvocation = node;
            }
            else if (methodSymbol?.Name == "AddHandler")
            {
                AddHandlerInvocation = node;
            }
            else if (methodSymbol?.Name == "AddDispatcher" && methodSymbol.IsGenericMethod)
            {
                AddDispatcherInvocation = node;
            }

            base.VisitInvocationExpression(node);
        }
    }
}