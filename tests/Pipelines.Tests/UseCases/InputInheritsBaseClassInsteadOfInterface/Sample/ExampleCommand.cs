using Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Sample;

public record ExampleCommand(string Value) : BaseCommand;