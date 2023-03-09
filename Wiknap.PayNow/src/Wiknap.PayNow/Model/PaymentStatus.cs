using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public enum PaymentStatus
{
    NEW,
    PENDING,
    CONFIRMED,
    REJECTED,
    ERROR,
    ABANDONED,
    EXPIRED
}