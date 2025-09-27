using System.Diagnostics;
using System.Reflection;

namespace Wiknap.Email.DependencyInjection.OpenTelemetry;

public sealed class WiknapEmailInstrumentation : IDisposable
{
    private static readonly Assembly Assembly = typeof(WiknapEmailInstrumentation).Assembly;
    internal static readonly string ActivitySourceName = Assembly.GetName().Name!;
    private const string ActivitySourceVersion = "1.0.0";

    public WiknapEmailInstrumentation()
    {
        ActivitySource = new ActivitySource(ActivitySourceName, ActivitySourceVersion);
    }

    public ActivitySource ActivitySource { get; }

    public void Dispose() => ActivitySource.Dispose();
}
