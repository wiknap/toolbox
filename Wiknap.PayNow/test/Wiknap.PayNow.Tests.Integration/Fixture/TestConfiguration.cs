using Microsoft.Extensions.Configuration;

using Xunit;

namespace Wiknap.PayNow.Tests.Integration.Fixture;

public sealed class TestConfiguration
{
    public TestOptions StandardOptions { get; }
    public TestOptions WhiteLabelOptions { get; }

    public TestConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        StandardOptions = builder.Build().GetSection("Standard").Get<TestOptions>()!;
        WhiteLabelOptions = builder.Build().GetSection("WhiteLabel").Get<TestOptions>()!;
    }
}

[CollectionDefinition("Configuration")]
public class TestConfigurationCollection : ICollectionFixture<TestConfiguration>;
