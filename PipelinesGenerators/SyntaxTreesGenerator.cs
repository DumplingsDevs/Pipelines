using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PipelinesGenerators;

#pragma warning disable RS1035 // Do not use banned APIs for analyzers

[Generator(LanguageNames.CSharp)]
internal class SyntaxTreesGenerator : ISourceGenerator
{
    private const string AttributeName = "GenerateImplementation"; // Atrybut oznaczający interfejsy do wygenerowania

    public void Execute(GeneratorExecutionContext context)
    {
        LogSyntaxTrees(context);
    }

    private static void LogSyntaxTrees(GeneratorExecutionContext context)
    {
        // begin creating the source we'll inject into the users compilation
        StringBuilder sourceBuilder = new StringBuilder(@"
using System;
namespace Pipelines
{
    public static class HelloWorld
    {
        public static void SayHello() 
        {
            Console.WriteLine(""Hello from generated code!"");
            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");
");

        // using the context, get a list of syntax trees in the users compilation
        IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

        // add the filepath of each tree to the class we're building
        foreach (SyntaxTree tree in syntaxTrees)
        {
            sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
        }

        // finish creating the source to inject
        sourceBuilder.Append(@"
        }
    }
}");

        // inject the created source into the users compilation
        context.AddSource("helloWorldGenerated.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Tutaj można zarejestrować wszelkie niezbędne metadane
    }
}