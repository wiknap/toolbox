using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Path;


internal interface IPayNowApiPaymentsPathBuilder
{
    IPayNowApiPaymentPathBuilder AddPaymentPath(string paymentId);
    void AddPaymentMethodsPath(Currency currency);
}

internal interface IPayNowApiPaymentPathBuilder
{
    void AddPaymentStatusPath();
    void AddPaymentRefundsPath();
}

internal interface IPayNowApiPaymentRefundsPathBuilder
{
    IPayNowApiPaymentRefundPathBuilder AddRefundPath(string refundId);
}

internal interface IPayNowApiPaymentRefundPathBuilder
{
    void AddRefundStatusPath();
}