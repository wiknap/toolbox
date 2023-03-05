using Wiknap.PayNow.Model;

namespace Wiknap.PayNow;

public interface IPayNowClient
{
    Task<PostPaymentResponse> PostPaymentRequestAsync(PostPaymentRequest paymentRequest);
    Task<GetPaymentStatusResponse> GetPaymentStatusAsync(string paymentId);
    Task<GetPaymentMethodsResponse> GetPaymentMethodsAsync(Currency currency = Currency.PLN);
    Task<PostRefundResponse> PostRefundRequestAsync(string paymentId, PostRefundRequest refundRequest);
    Task<GetRefundStatusResponse> GetRefundStatusAsync(string refundId);
}