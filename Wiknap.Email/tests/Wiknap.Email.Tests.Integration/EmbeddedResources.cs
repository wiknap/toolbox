using System.Reflection;

namespace Wiknap.Email.Tests.Integration;

internal static class EmbeddedResources
{
    internal static Stream Get(params string[] resourcePathParts)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{assembly.GetName().Name}.Resources.{string.Join(".", resourcePathParts)}";
        var stream = assembly.GetManifestResourceStream(resourceName);
        return stream ?? throw new Exception($"Resource with name {resourceName} not found");
    }

    internal static Stream GetTestPngImage() => Get("Images", "image.png");
}
