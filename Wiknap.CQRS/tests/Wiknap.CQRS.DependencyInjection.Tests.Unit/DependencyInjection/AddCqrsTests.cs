using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Wiknap.CQRS.DependencyInjection.Tests.Unit.DependencyInjection;

public sealed class AddCqrsTests
{
    [Fact]
    public void Given_NoAssembly_When_AddCqrs_Then_AddsHandlersFromCallingAssembly()
    {
        // Arrange
        var sc = new ServiceCollection();

        // Act
        sc.AddCqrs();
        var p = sc.BuildServiceProvider();
        var commandDispatcher = p.GetService<ICommandDispatcher>();
        var queryDispatcher = p.GetService<IQueryDispatcher>();
        var callingAssemblyCommandHandler = p.GetService<ICommandHandler<TestCommand>>();
        var callingAssemblyQueryHandler = p.GetService<IQueryHandler<TestQuery, int>>();
        var referenceAssemblyCommandHandler = p.GetService<ICommandHandler<Wiknap.CQRS.Tests.Unit.TestProject.TestCommand>>();
        var referenceAssemblyQueryHandler = p.GetService<IQueryHandler<Wiknap.CQRS.Tests.Unit.TestProject.TestQuery, int>>();

        // Assert
        commandDispatcher.ShouldNotBeNull();
        queryDispatcher.ShouldNotBeNull();
        callingAssemblyCommandHandler.ShouldNotBeNull();
        callingAssemblyQueryHandler.ShouldNotBeNull();
        referenceAssemblyCommandHandler.ShouldBeNull();
        referenceAssemblyQueryHandler.ShouldBeNull();
    }

    [Fact]
    public void Given_Assembly_When_AddCqrs_Then_AddsHandlersFromProvidedAssembly()
    {
        // Arrange
        var sc = new ServiceCollection();

        // Act
        sc.AddCqrs(typeof(Wiknap.CQRS.Tests.Unit.TestProject.TestCommand).Assembly);
        var p = sc.BuildServiceProvider();
        var callingAssemblyCommandHandler = p.GetService<ICommandHandler<TestCommand>>();
        var callingAssemblyQueryHandler = p.GetService<IQueryHandler<TestQuery, int>>();
        var referenceAssemblyCommandHandler = p.GetService<ICommandHandler<Wiknap.CQRS.Tests.Unit.TestProject.TestCommand>>();
        var referenceAssemblyQueryHandler = p.GetService<IQueryHandler<Wiknap.CQRS.Tests.Unit.TestProject.TestQuery, int>>();

        // Assert
        callingAssemblyCommandHandler.ShouldBeNull();
        callingAssemblyQueryHandler.ShouldBeNull();
        referenceAssemblyCommandHandler.ShouldNotBeNull();
        referenceAssemblyQueryHandler.ShouldNotBeNull();
    }
}
