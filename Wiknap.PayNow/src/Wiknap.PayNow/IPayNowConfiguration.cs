namespace Wiknap.PayNow;

public interface IPayNowConfiguration
{
    public string ApiKey { get; }
    public string SignatureKey { get; }
    public string ApiPath { get; }
}