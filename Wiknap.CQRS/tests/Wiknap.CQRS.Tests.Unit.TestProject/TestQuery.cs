namespace Wiknap.CQRS.Tests.Unit.TestProject;

public sealed class TestQuery : IQuery<int>
{
}

public sealed class TestQueryHandler : IQueryHandler<TestQuery, int>
{
    public Task<int> HandleAsync(TestQuery query, CancellationToken cancellationToken) => Task.FromResult(1);
}
