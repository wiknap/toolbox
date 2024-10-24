using JetBrains.Annotations;

using Wiknap.PayNow.Model;

namespace Wiknap.PayNow;

[PublicAPI]
public interface IPayNowClient
{
    Task<PostPaymentResponse>
        PostPaymentRequestAsync(PostPaymentRequest paymentRequest, CancellationToken ct = default);

    Task<GetPaymentStatusResponse> GetPaymentStatusAsync(string paymentId, CancellationToken ct = default);

    Task<IReadOnlyCollection<AvailablePaymentMethod>> GetPaymentMethodsAsync(Currency currency = Currency.PLN,
        CancellationToken ct = default);

    Task<PostRefundResponse> PostRefundRequestAsync(string paymentId, PostRefundRequest refundRequest,
        CancellationToken ct = default);

    Task<GetRefundStatusResponse> GetRefundStatusAsync(string refundId, CancellationToken ct = default);
}
