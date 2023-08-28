namespace Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

/// <summary>
/// 
/// </summary>
/// <param name="Id">Created or updated entity. In case of command that will schedule background job, then Id will equal background job id.
/// If null returned command does not have affect to any entity(edge case)</param>
public record CommandResult(string? Id);
