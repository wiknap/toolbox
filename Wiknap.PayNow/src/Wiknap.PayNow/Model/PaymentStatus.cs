namespace Wiknap.PayNow.Model;

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