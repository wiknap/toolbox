using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Shouldly;

using Xunit;

namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed class QueryDispatcherTests
{
    [Fact]
    public async Task Given_Query_When_DispatchAsyncWithExplicitGenerics_Then_ShouldCallHandlerAndReturnResult()
    {
        // Arrange
        var sc = new ServiceCollection();
        const int queryResult = 1;
        var query = new TestQuery();
        var queryHandler = Substitute.For<IQueryHandler<TestQuery, int>>();
        queryHandler.HandleAsync(query).Returns(queryResult);
        sc.AddSingleton<IQueryHandler<TestQuery, int>>(_ => queryHandler);
        var queryDispatcher = new QueryDispatcher(sc.BuildServiceProvider());

        // Act
        var result = await queryDispatcher.DispatchAsync<TestQuery, int>(query);

        // Assert
        result.ShouldBe(1);
        await queryHandler.Received().HandleAsync(query);
    }

    [Fact]
    public async Task Given_Query_When_DispatchAsyncWithImplicitGenerics_Then_ShouldCallHandlerAndReturnResult()
    {
        // Arrange
        var sc = new ServiceCollection();
        const int queryResult = 1;
        var query = new TestQuery();
        var queryHandler = Substitute.For<IQueryHandler<TestQuery, int>>();
        queryHandler.HandleAsync(query).Returns(queryResult);
        sc.AddSingleton<IQueryHandler<TestQuery, int>>(_ => queryHandler);
        var queryDispatcher = new QueryDispatcher(sc.BuildServiceProvider());

        // Act
        var result = await queryDispatcher.DispatchAsync(query);

        // Assert
        result.ShouldBe(1);
        await queryHandler.Received().HandleAsync(query);
    }
}
