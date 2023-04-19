using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Wiknap.ApplePay.Blazor.Components;
using Wiknap.ApplePay.Model;

namespace Wiknap.ApplePay.Blazor;

public class ApplePayJs : IApplePayJs
{
    private readonly IJSRuntime jsRuntime;

    private readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ApplePayJs(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task AddOnClickAsync(ElementReference elemRef, DotNetObjectReference<ApplePayButton> objRef)
    {
        await jsRuntime.InvokeVoidAsync("blazorApplePay.addOnClick", elemRef, objRef);
    }

    public async Task CreateSessionAsync(Session session, DotNetObjectReference<ApplePayButton> objRef)
    {
        await jsRuntime.InvokeVoidAsync("blazorApplePay.createSession", JsonSerializer.Serialize(session, options),
            objRef);
    }
}

public interface IApplePayJs
{
    Task AddOnClickAsync(ElementReference elemRef, DotNetObjectReference<ApplePayButton> objRef);
    Task CreateSessionAsync(Session session, DotNetObjectReference<ApplePayButton> objRef);
}