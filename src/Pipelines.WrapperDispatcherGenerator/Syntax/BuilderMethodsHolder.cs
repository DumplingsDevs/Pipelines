using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pipelines.WrapperDispatcherGenerator.Syntax;

public record BuilderMethodsHolder(InvocationExpressionSyntax AddInputInvocation, InvocationExpressionSyntax AddHandlerInvocation, InvocationExpressionSyntax AddDispatcherInvocation)
{
    public InvocationExpressionSyntax AddInputInvocation { get; } = AddInputInvocation;
    public InvocationExpressionSyntax AddHandlerInvocation { get; } = AddHandlerInvocation;
    public InvocationExpressionSyntax AddDispatcherInvocation { get; } = AddDispatcherInvocation;
}