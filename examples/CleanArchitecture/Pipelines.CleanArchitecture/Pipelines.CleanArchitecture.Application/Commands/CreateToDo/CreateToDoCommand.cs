using Pipelines.CleanArchitecture.Abstractions.Commands;

namespace Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

public record CreateToDoCommand(string Title) : ICommand;