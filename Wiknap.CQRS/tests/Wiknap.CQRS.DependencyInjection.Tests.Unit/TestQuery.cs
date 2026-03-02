namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed record TestQuery : IQuery<int>;

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, int>
{
    public Task<int> HandleAsync(TestQuery query, CancellationToken cancellationToken) => Task.FromResult(1);
}
