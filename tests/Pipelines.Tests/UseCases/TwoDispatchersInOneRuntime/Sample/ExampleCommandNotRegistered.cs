using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

internal record ExampleCommandNotRegistered(string Value) : ICommand<CommandResult>
{
    
}
