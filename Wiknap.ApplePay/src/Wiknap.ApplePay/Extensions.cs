using Microsoft.Extensions.DependencyInjection;
using Wiknap.ApplePay.Adyen;

namespace Wiknap.ApplePay;

public static class Extensions
{
    public static IServiceCollection AddApplePay(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MerchantCertificate>();
        serviceCollection.AddHttpClient<IApplePayClient, ApplePayClient>("ApplePay").ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            var merchantCertificate = serviceProvider.GetRequiredService<MerchantCertificate>();
            var certificate = merchantCertificate.GetCertificate();
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            return handler;
        });

        serviceCollection.AddHttpClient<IAdyenClient, AdyenClient>("Adyen").ConfigureHttpClient(client =>
        {
            var key =
                "AQEqhmfuXNWTK0Qc+iSHm28sqPa1SZtECIFLVGS+PEWAhBFlrLsQEThqmY3REMFdWw2+5HzctViMSCJMYAc=-ZxpIoASneKu1rYuDuYjeU/IuVqWAv1PLvietsUI2lMs=-4AJ4fh)#>+.[SEQA";
            client.DefaultRequestHeaders.Add("x-API-key", key);
        });

        return serviceCollection;
    }
}