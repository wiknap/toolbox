using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class AuthorizationTypeJsonConverter : CustomEnumJsonConverter<AuthorizationType>
{
    public AuthorizationTypeJsonConverter()
        : base(new Dictionary<AuthorizationType, string>
        {
            { AuthorizationType.Redirect, "REDIRECT" },
            { AuthorizationType.Code, "CODE" }
        })
    {
    }
}
