namespace Wiknap.CQRS.Tests.Unit.TestProject;

public sealed class TestCommand : ICommand
{
}

public sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
