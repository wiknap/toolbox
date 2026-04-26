using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.CQRS.DependencyInjection;

internal sealed class
    AsyncEnumerableQueryHandlerWrapper<TQuery, TResponse> : AsyncEnumerableQueryHandlerWrapper<TResponse>
    where TQuery : class, IAsyncEnumerableQuery<TResponse>
{
    internal override IAsyncEnumerable<TResponse> Handle(IAsyncEnumerableQuery<TResponse> query,
        IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IAsyncEnumerableQueryHandler<TQuery, TResponse>>();
        var queryCast = query as TQuery ?? throw new InvalidOperationException();
        return handler.HandleAsync(queryCast, cancellationToken);
    }
}

internal abstract class AsyncEnumerableQueryHandlerWrapper<TResponse>
{
    internal abstract IAsyncEnumerable<TResponse> Handle(
        IAsyncEnumerableQuery<TResponse> query,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}
