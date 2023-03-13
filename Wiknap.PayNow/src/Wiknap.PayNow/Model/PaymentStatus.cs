using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
[JsonConverter(typeof(PaymentStatusJsonConverter))]
public enum PaymentStatus
{
    New,
    Pending,
    Confirmed,
    Rejected,
    Error,
    Abandoned,
    Expired
}