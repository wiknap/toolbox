using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Wiknap.ApplePay;

public class MerchantCertificate
{
    public X509Certificate2 GetCertificate()
    {
        var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "merchant_id.pfx");
        return LoadCertificateFromDisk(fileName, "haslo123");
    }

    public string GetMerchantIdentifier()
    {
        try
        {
            using var merchantCertificate = GetCertificate();
            return GetMerchantIdentifier(merchantCertificate);
        }
        catch (InvalidOperationException)
        {
            return string.Empty;
        }
    }

    private static string GetMerchantIdentifier(X509Certificate2 certificate)
    {
        // This OID returns the ASN.1 encoded merchant identifier
        var extension = certificate.Extensions["1.2.840.113635.100.6.32"];

        if (extension == null)
        {
            return string.Empty;
        }

        // Convert the raw ASN.1 data to a string containing the ID
        return Encoding.ASCII.GetString(extension.RawData)[2..];
    }

    private static X509Certificate2 LoadCertificateFromDisk(string? fileName, string? password)
    {
        try
        {
            return new X509Certificate2(
                fileName ?? string.Empty,
                password);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load Apple Pay merchant certificate file from '{fileName}'.", ex);
        }
    }
}
