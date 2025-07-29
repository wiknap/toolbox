using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.CQRS.DependencyInjection;

internal sealed class QueryHandlerWrapper<TQuery, TResponse> : QueryHandlerWrapper<TResponse> where TQuery : class, IQuery<TResponse>
{
    internal override Task<TResponse> Handle(IQuery<TResponse> query, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        var queryCast = query as TQuery ?? throw new InvalidOperationException();
        return handler.HandleAsync(queryCast, cancellationToken);
    }
}

internal abstract class QueryHandlerWrapper<TResponse>
{
    internal abstract Task<TResponse> Handle(IQuery<TResponse> query, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}
