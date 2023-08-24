using System.Text;

namespace Pipelines.Benchmarks;

internal class Samples
{
    internal static void GeneratePipelinesSamples()
    {
        var count = 200;
        var rootPath =
            Path.GetDirectoryName(
                Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));

        using var writer = new StreamWriter($"{rootPath}/Requests.cs");
        writer.WriteLine("using Pipelines.Benchmarks.Types;");
        writer.WriteLine("namespace Pipelines.Benchmarks.Sample;");
        for (int i = 0; i < count; i++)
        {
            writer.WriteLine($"public record ExampleRequest{i + 1}(string Value) : IRequest<ExampleCommandResult>;");
        }

        using var writer2 = new StreamWriter($"{rootPath}/RequestsMediator.cs");
        writer2.WriteLine("using MediatR;");
        writer2.WriteLine("namespace Pipelines.Benchmarks.Sample.Mediator;");
        for (int i = 0; i < count; i++)
        {
            writer2.WriteLine(
                $"public record MediatorExampleRequest{i}(string Value) : IRequest<ExampleCommandResult>;");
        }

        using var writer3 = new StreamWriter($"{rootPath}/RequestHandlersMediator.cs");
        writer3.WriteLine("using MediatR;");
        writer3.WriteLine("namespace Pipelines.Benchmarks.Sample.Mediator;");
        for (int i = 0; i < count; i++)
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

        using var writer4 = new StreamWriter($"{rootPath}/RequestHandlersMediator.cs");
        writer4.WriteLine("using Pipelines.Benchmarks.Types;");
        writer4.WriteLine("namespace Pipelines.Benchmarks.Sample;");
        for (int i = 0; i < count; i++)
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
    }
}