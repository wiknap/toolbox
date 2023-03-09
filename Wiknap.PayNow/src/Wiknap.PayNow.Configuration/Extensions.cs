using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.PayNow.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPayNow(this IServiceCollection services)
    {
        services.AddOptions<PayNowOptions>().BindConfiguration(PayNowOptions.SectionName);
        services.AddSingleton<IPayNowConfiguration, PayNowConfiguration>();
        services.AddScoped<IPayNowClient, PayNowClient>();

        return services;
    }
}