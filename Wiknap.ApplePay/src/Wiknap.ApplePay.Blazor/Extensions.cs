using Microsoft.Extensions.DependencyInjection;

namespace Wiknap.ApplePay.Blazor;

public static class Extensions
{
    public static IServiceCollection AddApplePay(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IApplePayJs, ApplePayJs>()
            .AddScoped<IApplePayClient, ApplePayClient>();
    }
}