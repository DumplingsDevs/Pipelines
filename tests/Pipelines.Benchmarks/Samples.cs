using System.Text;

namespace Pipelines.Benchmarks;

internal class Samples
{
    internal static void GenerateRegistrations()
    {
        var count = 500;
        var rootPath =
            Path.GetDirectoryName(
                Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));

        using var writer = new StreamWriter($"{rootPath}/Registrations.cs");
        for (int i = 1; i <= count; i++)
        {
            writer.WriteLine($"RegisterHandler<ExampleRequest{i}, ExampleCommandResult>(serviceProvider);");

        }
    }

    internal static void GeneratePipelinesSamples()
    {
        var count = 500;
        var rootPath =
            Path.GetDirectoryName(
                Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));

        using var writer = new StreamWriter($"{rootPath}/Requests.cs");
        writer.WriteLine("using Pipelines.Benchmarks.Types;");
        writer.WriteLine("namespace Pipelines.Benchmarks.Sample;");
        for (int i = 1; i <= count; i++)
        {
            writer.WriteLine($"public record ExampleRequest{i}(string Value) : IRequest<ExampleCommandResult>;");
        }

        using var writer2 = new StreamWriter($"{rootPath}/RequestsMediator.cs");
        writer2.WriteLine("using MediatR;");
        writer2.WriteLine("namespace Pipelines.Benchmarks.Sample.Mediator;");
        for (int i = 1; i <= count; i++)
        {
            writer2.WriteLine(
                $"public record MediatorExampleRequest{i}(string Value) : IRequest<ExampleCommandResult>;");
        }

        using var writer3 = new StreamWriter($"{rootPath}/RequestHandlersMediator.cs");
        writer3.WriteLine("using MediatR;");
        writer3.WriteLine("namespace Pipelines.Benchmarks.Sample.Mediator;");
        for (int i = 1; i <= count; i++)
        {
            writer3.WriteLine($@"
public class MediatorExampleRequestHandler{i} : IRequestHandler<MediatorExampleRequest{i}, ExampleCommandResult>
{{
    public Task<ExampleCommandResult> Handle(MediatorExampleRequest{i} request, CancellationToken cancellationToken)
    {{
        return Task.FromResult(new ExampleCommandResult(request.Value + "" Changed""));
    }}
}}
");
        }

        using var writer4 = new StreamWriter($"{rootPath}/RequestHandlers.cs");
        writer4.WriteLine("using Pipelines.Benchmarks.Types;");
        writer4.WriteLine("namespace Pipelines.Benchmarks.Sample;");
        for (int i = 1; i <= count; i++)
        {
            writer4.WriteLine($@"
public class ExampleRequestHandler{i} : IRequestHandler<ExampleRequest{i}, ExampleCommandResult>
{{
    public Task<ExampleCommandResult> HandleAsync(ExampleRequest{i} request, CancellationToken token)
    {{
        return Task.FromResult(new ExampleCommandResult(request.Value + "" Changed""));
    }}
}}
");
        }
        
        using var writer5 = new StreamWriter($"{rootPath}/RequestDefinitions.cs");  
        writer5.WriteLine("private readonly List<Types.IRequest<ExampleCommandResult>> _pipelinesRequests = new()");
        writer5.WriteLine("{");
        for (int i = 1; i < count; i += 20)
        {
            writer5.WriteLine($@"    new ExampleRequest{i}(""My test request {i}""),");
        }
        writer5.WriteLine("};");
        
        using var writer6 = new StreamWriter($"{rootPath}/MediatorRequestDefinitions.cs");  
        writer6.WriteLine("private readonly List<MediatR.IRequest<ExampleCommandResult>> _mediatorRequests = new()");
        writer6.WriteLine("{");
        for (int i = 1; i < count; i += 20)
        {
            writer6.WriteLine($@"    new MediatorExampleRequest{i}(""My test request {i}""),");
        }
        writer6.WriteLine("};");
    }
}