namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public class TestQuery : IQuery<int>
{
}

public class TestQueryHandler : IQueryHandler<TestQuery, int>
{
    public Task<int> HandleAsync(TestQuery query, CancellationToken cancellationToken) => Task.FromResult(1);
}
