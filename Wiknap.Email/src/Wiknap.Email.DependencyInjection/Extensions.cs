using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Trace;

using Wiknap.Email.DependencyInjection.Decorators;
using Wiknap.Email.DependencyInjection.OpenTelemetry;

namespace Wiknap.Email.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services
            .AddScoped<IEmailClient, EmailClient>()
            .Decorate<IEmailClient, EmailClientLoggingDecorator>()
            .Decorate<IEmailClient, EmailClientFilterDecorator>();

        return services;
    }

    public static TracerProviderBuilder AddWiknapEmailInstrumentation(
        this TracerProviderBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton<WiknapEmailInstrumentation>()
                .Decorate<IEmailClient, EmailClientTracingDecorator>();
        });

        builder.AddInstrumentation(sp => sp.GetRequiredService<WiknapEmailInstrumentation>());

        builder.AddSource(WiknapEmailInstrumentation.ActivitySourceName);

        return builder;
    }
}
