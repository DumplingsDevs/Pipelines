using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pipelines.NativeDispatcherGenerator.Syntax;

internal class SyntaxReceiver : CSharpSyntaxWalker
{
    private readonly Compilation _compilation;

    public SyntaxReceiver(Compilation compilation, SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node) : base(depth)
    {
        _compilation = compilation;
    }

    private InvocationExpressionSyntax? AddInputInvocation { get; set; }
    private InvocationExpressionSyntax? AddHandlerInvocation { get; set; }
    private InvocationExpressionSyntax? AddDispatcherInvocation { get; set; }

    public List<BuilderMethodsHolder> BuilderInvocations { get; private set; } = new();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        // Visit each invocation expression and check if it matches our methods
        var methodSymbol =
            ModelExtensions.GetSymbolInfo(_compilation.GetSemanticModel(node.SyntaxTree), node).Symbol as
                IMethodSymbol;
        switch (methodSymbol?.Name)
        {
            case "AddInput":
                AddInputInvocation = node;
                break;
            case "AddHandler":
                AddHandlerInvocation = node;
                break;
            case "AddDispatcher" when methodSymbol.IsGenericMethod:
                AddDispatcherInvocation = node;
                break;
        }

        if (AddInputInvocation is not null && AddHandlerInvocation is not null && AddDispatcherInvocation is not null)
        {
            BuilderInvocations.Add(new BuilderMethodsHolder(AddInputInvocation, AddHandlerInvocation,
                AddDispatcherInvocation));
            AddInputInvocation = null;
            AddHandlerInvocation = null;
            AddDispatcherInvocation = null;
        }

        base.VisitInvocationExpression(node);
    }
}