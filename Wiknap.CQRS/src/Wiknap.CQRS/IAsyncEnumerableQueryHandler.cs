namespace Wiknap.CQRS;

public interface IAsyncEnumerableQueryHandler<in TQuery, out TResult>
    where TQuery : class, IAsyncEnumerableQuery<TResult>
{
    IAsyncEnumerable<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
