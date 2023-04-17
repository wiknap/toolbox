using System.Globalization;

namespace Wiknap.ApplePay.Model;

public record LineItem
{
    public LineItem(string label, decimal amount)
    {
        Label = label;
        Amount = amount.ToString("0.00", CultureInfo.InvariantCulture);
    }

    public string Label { get; }
    public string Amount { get; }

    public void Deconstruct(out string label, out decimal amount)
    {
        label = Label;
        amount = decimal.Parse(Amount, CultureInfo.InvariantCulture);
    }
}