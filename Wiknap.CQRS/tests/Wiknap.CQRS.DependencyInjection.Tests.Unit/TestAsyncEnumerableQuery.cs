using System.Runtime.CompilerServices;

namespace Wiknap.CQRS.DependencyInjection.Tests.Unit;

public sealed record TestAsyncEnumerableQuery : IAsyncEnumerableQuery<int>;

internal sealed class TestAsyncEnumerableQueryHandler : IAsyncEnumerableQueryHandler<TestAsyncEnumerableQuery, int>
{
    public async IAsyncEnumerable<int> HandleAsync(TestAsyncEnumerableQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return 1;
    }
}
