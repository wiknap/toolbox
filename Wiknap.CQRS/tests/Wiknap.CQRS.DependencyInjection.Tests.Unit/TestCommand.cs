namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public class TestCommand : ICommand
{
}

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
