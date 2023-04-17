using Microsoft.AspNetCore.Mvc;
using Wiknap.ApplePay;
using Wiknap.ApplePay.Adyen;
using Wiknap.ApplePay.Model;
using PaymentRequest = Wiknap.ApplePay.Adyen.PaymentRequest;

namespace BlazorApplePay.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplePayController
{
    private readonly MerchantCertificate certificate;
    private readonly IApplePayClient applePayClient;
    private readonly IAdyenClient adyenClient;

    public ApplePayController(MerchantCertificate certificate, IApplePayClient applePayClient, IAdyenClient adyenClient)
    {
        this.certificate = certificate;
        this.applePayClient = applePayClient;
        this.adyenClient = adyenClient;
    }

    [HttpPost("session/start")]
    public async Task<object> StartSession([FromBody] string validationUrl, [FromHeader(Name = "Host")] string host,
        CancellationToken cancellationToken)
    {
        var request = new MerchantSessionRequest
        {
            DisplayName = "Apple Pay",
            Initiative = "web",
            InitiativeContext = host,
            MerchantIdentifier = certificate.GetMerchantIdentifier(),
        };

        var merchantSession = await applePayClient.GetMerchantSessionAsync(validationUrl, request, cancellationToken);
        return merchantSession.RootElement;
    }

    [HttpPost("payment")]
    public async Task<bool> Payment([FromBody] string token)
    {
        return await adyenClient.PostPaymentAsync(new PaymentRequest(new Amount("USD", 1000), Guid.NewGuid().ToString(),
            new PaymentMethod("applepay", token), "https://app-applepay.azurewebsites.net/", "WiktorNapieralaECOM"));
    }
}