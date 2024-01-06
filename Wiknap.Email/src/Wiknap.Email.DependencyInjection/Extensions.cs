using Microsoft.Extensions.DependencyInjection;
using Wiknap.Email.DependencyInjection.Decorators;

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
}