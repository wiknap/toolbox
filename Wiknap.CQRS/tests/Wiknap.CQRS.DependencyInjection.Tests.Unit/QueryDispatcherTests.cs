using Microsoft.Extensions.DependencyInjection;

using Wiknap.CQRS.DependencyInjection;

using NSubstitute;

using Shouldly;

using Xunit;

namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed class QueryDispatcherTests
{
    [Fact]
    public async Task Given_Query_When_DispatchAsync_Then_ShouldCallHandlerAndReturnResult()
    {
        // Arrange
        var sc = new ServiceCollection();
        const int queryResult = 1;
        var query = new TestQuery();
        var queryHandler = Substitute.For<IQueryHandler<TestQuery, int>>();
        queryHandler.HandleAsync(query).Returns(queryResult);
        sc.AddSingleton<IQueryHandler<TestQuery, int>>(_ => queryHandler);
        var queryDispatcher = new CQRS.DependencyInjection.QueryDispatcher(sc.BuildServiceProvider());

        // Act
        var result = await queryDispatcher.DispatchAsync<TestQuery, int>(query);

        // Assert
        result.ShouldBe(1);
        await queryHandler.Received().HandleAsync(query);
    }
}
