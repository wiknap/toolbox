using System.Globalization;
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
    private readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture;

    public PayNowApiPathBuilder()
    {
        builder = new StringBuilder();
        AppendPath("v1");
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
        builder.Append(cultureInfo, $"?currency={currency.ToString()}");
    }

    private void AppendPath(string path) => builder.Append(cultureInfo, $"/{path}");

    public override string ToString() => builder.ToString();
}
