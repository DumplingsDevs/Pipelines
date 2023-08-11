using Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Sample;

public record ExampleCommand2(string Value) : BaseCommand;