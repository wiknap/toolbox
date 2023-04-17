using Microsoft.JSInterop;
using Wiknap.ApplePay.Blazor.Components;
using Wiknap.ApplePay.Model;

namespace Wiknap.ApplePay.Blazor;

public class ApplePayJs : IApplePayJs
{
    private readonly IJSRuntime jsRuntime;

    public ApplePayJs(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task AddOnClickAsync(string id, DotNetObjectReference<ApplePayButton> objRef)
    {
        await jsRuntime.InvokeVoidAsync("blazorApplePay.addOnClick", id, objRef);
    }

    public async Task CreateSessionAsync(Session session, DotNetObjectReference<ApplePayButton> objRef)
    {
        await jsRuntime.InvokeVoidAsync("blazorApplePay.createSession", session, objRef);
    }
}

public interface IApplePayJs
{
    Task AddOnClickAsync(string id, DotNetObjectReference<ApplePayButton> objRef);
    Task CreateSessionAsync(Session session, DotNetObjectReference<ApplePayButton> objRef);
}