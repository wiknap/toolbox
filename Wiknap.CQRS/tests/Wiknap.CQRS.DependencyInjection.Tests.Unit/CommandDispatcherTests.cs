using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed class CommandDispatcherTests
{
    [Fact]
    public async Task Given_Command_When_DispatchAsync_Then_ShouldCallHandler()
    {
        // Arrange
        var sc = new ServiceCollection();
        var commandHandler = Substitute.For<ICommandHandler<TestCommand>>();
        sc.AddSingleton<ICommandHandler<TestCommand>>(_ => commandHandler);
        var commandDispatcher = new CommandDispatcher(sc.BuildServiceProvider());
        var command = new TestCommand();

        // Act
        await commandDispatcher.DispatchAsync(command);

        // Assert
        await commandHandler.Received().HandleAsync(command);
    }
}
