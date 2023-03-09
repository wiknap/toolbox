using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.PayNow.Configuration;

[UsedImplicitly]
public static class ServiceCollectionExtension
{
    [PublicAPI]
    public static IServiceCollection AddPayNow(this IServiceCollection services)
    {
        services.AddOptions<PayNowOptions>().BindConfiguration(PayNowOptions.SectionName);
        services.AddSingleton<IPayNowConfiguration, PayNowConfiguration>();
        services.AddScoped<IPayNowClient, PayNowClient>();

        return services;
    }
}