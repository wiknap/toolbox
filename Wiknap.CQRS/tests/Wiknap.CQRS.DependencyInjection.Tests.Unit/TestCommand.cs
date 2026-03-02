namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed record TestCommand : ICommand;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
