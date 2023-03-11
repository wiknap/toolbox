using System.Security.Cryptography;

namespace Wiknap.PayNow;

internal sealed class HmacSha256Calculator : IDisposable
{
    private readonly HMACSHA256 hmacSha256;

    public HmacSha256Calculator(byte[] keyBytes)
    {
        hmacSha256 = new HMACSHA256(keyBytes);
    }

    public string CalculateHmac(byte[] byteArray) => ConvertToBase64String(hmacSha256.ComputeHash(byteArray));

    private static string ConvertToBase64String(byte[] byteArray) => Convert.ToBase64String(byteArray);

    public void Dispose()
    {
        hmacSha256.Dispose();
    }
}