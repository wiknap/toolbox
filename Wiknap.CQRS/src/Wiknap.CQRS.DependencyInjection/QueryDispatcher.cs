using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.CQRS.DependencyInjection;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query,
        CancellationToken cancellationToken = default) where TQuery : class, IQuery<TQueryResult>
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.HandleAsync(query, cancellationToken);
    }

    public Task<TQueryResult> DispatchAsync<TQueryResult>(IQuery<TQueryResult> query,
        CancellationToken cancellationToken = default)
    {
        var wrapperType = typeof(QueryHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TQueryResult));
        var instance = Activator.CreateInstance(wrapperType);
        var wrapper = instance as QueryHandlerWrapper<TQueryResult> ?? throw new InvalidOperationException();
        return wrapper.Handle(query, _serviceProvider, cancellationToken);
    }

    public IAsyncEnumerable<TQueryResult> DispatchAsync<TQueryResult>(IAsyncEnumerableQuery<TQueryResult> query,
        CancellationToken cancellationToken = default)
    {
        var wrapperType =
            typeof(AsyncEnumerableQueryHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TQueryResult));
        var instance = Activator.CreateInstance(wrapperType);
        var wrapper = instance as AsyncEnumerableQueryHandlerWrapper<TQueryResult> ??
                      throw new InvalidOperationException();
        return wrapper.Handle(query, _serviceProvider, cancellationToken);
    }
}
