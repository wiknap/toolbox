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

    [Fact]
    public async Task Given_AsyncEnumerableQuery_When_DispatchAsync_Then_ShouldCallHandlerAndReturnResults()
    {
        // Arrange
        var sc = new ServiceCollection();
        var query = new TestAsyncEnumerableQuery();
        var queryHandler = Substitute.For<IAsyncEnumerableQueryHandler<TestAsyncEnumerableQuery, int>>();
        queryHandler.HandleAsync(query).Returns(GetItems());
        sc.AddSingleton<IAsyncEnumerableQueryHandler<TestAsyncEnumerableQuery, int>>(_ => queryHandler);
        var queryDispatcher = new QueryDispatcher(sc.BuildServiceProvider());

        // Act
        var asyncEnumerable = queryDispatcher.DispatchAsync(query);

        // Assert
        var items = new List<int>();
        await foreach (var item in asyncEnumerable)
            items.Add(item);
        items.ShouldBe([1]);
        queryHandler.Received().HandleAsync(query);
        return;

        static async IAsyncEnumerable<int> GetItems()
        {
            yield return 1;
        }
    }
}
