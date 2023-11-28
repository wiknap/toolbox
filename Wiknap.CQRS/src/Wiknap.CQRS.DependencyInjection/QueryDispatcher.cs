using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.CQRS.DependencyInjection;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query,
        CancellationToken cancellationToken = default) where TQuery : class, IQuery<TQueryResult>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.HandleAsync(query, cancellationToken);
    }
}
