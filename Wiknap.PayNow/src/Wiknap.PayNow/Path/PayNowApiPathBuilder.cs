using System.Text;
using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Path;

internal sealed class PayNowApiPathBuilder :
    IPayNowApiPaymentsPathBuilder,
    IPayNowApiPaymentPathBuilder,
    IPayNowApiPaymentRefundsPathBuilder,
    IPayNowApiPaymentRefundPathBuilder
{
    private readonly StringBuilder builder;

    public PayNowApiPathBuilder()
    {
        builder = new StringBuilder();
        AppendPath("api");
    }

    public IPayNowApiPaymentsPathBuilder AddPaymentsPath()
    {
        AppendPath("payments");
        return this;
    }

    public IPayNowApiPaymentRefundsPathBuilder AddPaymentRefundsPath()
    {
        AppendPath("refunds");
        return this;
    }

    void IPayNowApiPaymentPathBuilder.AddPaymentStatusPath() => AppendPath("status");
    void IPayNowApiPaymentPathBuilder.AddPaymentRefundsPath() => AppendPath("refunds");
    void IPayNowApiPaymentRefundPathBuilder.AddRefundStatusPath() => AppendPath("status");

    IPayNowApiPaymentRefundPathBuilder IPayNowApiPaymentRefundsPathBuilder.AddRefundPath(string refundId)
    {
        AppendPath(refundId);
        return this;
    }

    IPayNowApiPaymentPathBuilder IPayNowApiPaymentsPathBuilder.AddPaymentPath(string paymentId)
    {
        AppendPath(paymentId);
        return this;
    }

    void IPayNowApiPaymentsPathBuilder.AddPaymentMethodsPath(Currency currency)
    {
        AppendPath("paymentmethods");
        builder.Append($"?currency={currency.ToString()}");
    }

    private void AppendPath(string path) => builder.Append($"/{path}");

    public override string ToString() => builder.ToString();
}