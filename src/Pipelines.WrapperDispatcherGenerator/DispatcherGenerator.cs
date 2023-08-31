using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Pipelines.WrapperDispatcherGenerator.Extensions;
using Pipelines.WrapperDispatcherGenerator.Builders;
using Pipelines.WrapperDispatcherGenerator.Exceptions;
using Pipelines.WrapperDispatcherGenerator.Models;
using Pipelines.WrapperDispatcherGenerator.Syntax;

#pragma warning disable RS1035 // Do not use banned APIs for analyzers

namespace Pipelines.WrapperDispatcherGenerator;

// [Generator]
public class DispatcherGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var configs = GetPipelineConfigs(context).Distinct();

        foreach (var config in configs)
        {
            var interfaceName = config.DispatcherType.GetNameWithNamespace();

            var classSourceCode = BuildSourceCode(context, config);

            if (!string.IsNullOrEmpty(classSourceCode))
            {
                var sourceText = SourceText.From(classSourceCode, Encoding.UTF8);
                var sourceFileName = $"{interfaceName}Implementation.g.cs";

                context.AddSource(sourceFileName, sourceText);
            }
        }
    }

    private static string? BuildSourceCode(GeneratorExecutionContext context, PipelineConfig config)
    {
        try
        {
            var builder = new DispatcherImplementationBuilder(config, context);
            return builder.Build();
        }
        catch (GeneratorException e)
        {
            //TO DO: Report diagnostics using context.ReportDiagnostic
            return null;
        }
    }

    private IEnumerable<PipelineConfig> GetPipelineConfigs(GeneratorExecutionContext context)
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
            if (!syntaxReceiver.BuilderInvocations.Any()) continue;

            foreach (var (addInputInvocation, addHandlerInvocation, addDispatcherInvocation) in syntaxReceiver
                         .BuilderInvocations)
            {
                var inputType = GetTypeOfSymbol(context.Compilation, addInputInvocation, 0);
                var handlerType = GetTypeOfSymbol(context.Compilation, addHandlerInvocation, 1);
                var dispatcherType =
                    GetGenericSymbol(context.Compilation, addDispatcherInvocation, "AddDispatcher");

                if (inputType is null || handlerType is null || dispatcherType is null)
                {
                    //TO DO: Throws exception, but should should we show error message?
                    continue;
                }

                yield return new PipelineConfig(dispatcherType, handlerType, inputType);
            }
        }
    }

    private INamedTypeSymbol? GetGenericSymbol(Compilation compilation, InvocationExpressionSyntax invocationSyntax,
        string methodName)
    {
        var semanticModel = compilation.GetSemanticModel(invocationSyntax.SyntaxTree);
        var genericSyntax = invocationSyntax.DescendantNodes().OfType<GenericNameSyntax>().ToList();

        foreach (var genericNameSyntax in genericSyntax)
        {
            var typeInfo = semanticModel.GetSymbolInfo(genericNameSyntax);

            if (typeInfo is not { Symbol: IMethodSymbol methodSymbol }) continue;

            if (methodSymbol.Name.Contains(methodName))
            {
                return (INamedTypeSymbol)methodSymbol.TypeArguments.FirstOrDefault();
            }
        }

        return null;
    }

    private INamedTypeSymbol? GetTypeOfSymbol(Compilation compilation, InvocationExpressionSyntax invocationSyntax,
        int skip)
    {
        var typeOfSyntax = invocationSyntax.DescendantNodes().OfType<TypeOfExpressionSyntax>().Skip(skip)
            .FirstOrDefault();
        if (typeOfSyntax is null) return null;
        
        var semanticModel = compilation.GetSemanticModel(typeOfSyntax.SyntaxTree);

        var identifierSyntax = typeOfSyntax.ChildNodes().OfType<IdentifierNameSyntax>()
            .FirstOrDefault();

        if (identifierSyntax != null)
        {
            var typeInfo = semanticModel.GetTypeInfo(identifierSyntax);
            return (INamedTypeSymbol)typeInfo.Type!;
        }

        var generic = typeOfSyntax.ChildNodes().OfType<GenericNameSyntax>()
            .FirstOrDefault();

        if (generic != null)
        {
            var typeInfo = semanticModel.GetSymbolInfo(generic);
            return ((INamedTypeSymbol)typeInfo.Symbol).ConstructedFrom;
        }

        var childSyntaxType = typeOfSyntax.ChildNodes().FirstOrDefault();

        if (childSyntaxType != null)
        {
            var typeInfo = semanticModel.GetTypeInfo(childSyntaxType);
            return (INamedTypeSymbol)typeInfo.Type!;
        }

        return null;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}