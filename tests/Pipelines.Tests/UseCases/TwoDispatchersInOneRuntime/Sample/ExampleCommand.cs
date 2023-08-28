using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

internal record ExampleCommand(string Value) : ICommand<CommandResult>
{
    
}
