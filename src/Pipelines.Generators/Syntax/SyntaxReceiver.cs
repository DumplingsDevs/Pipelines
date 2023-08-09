using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pipelines.Generators.Syntax;

internal class SyntaxReceiver : CSharpSyntaxWalker
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
        var methodSymbol =
            ModelExtensions.GetSymbolInfo(_compilation.GetSemanticModel(node.SyntaxTree), node).Symbol as
                IMethodSymbol;
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