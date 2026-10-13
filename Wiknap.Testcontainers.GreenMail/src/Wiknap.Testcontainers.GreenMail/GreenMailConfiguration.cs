using Docker.DotNet.Models;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace Wiknap.Testcontainers.GreenMail;

public sealed class GreenMailConfiguration : ContainerConfiguration
{
    public GreenMailConfiguration(bool? authenticationDisabled = null, IEnumerable<GreenMailUser>? users = null)
    {
        AuthenticationDisabled = authenticationDisabled;
        Users = users;
    }

    public GreenMailConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public GreenMailConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public GreenMailConfiguration(GreenMailConfiguration resourceConfiguration)
        : this(new GreenMailConfiguration(), resourceConfiguration)
    {
    }

    public GreenMailConfiguration(GreenMailConfiguration oldValue, GreenMailConfiguration newValue)
        : base(oldValue, newValue)
    {
        AuthenticationDisabled = newValue.AuthenticationDisabled ?? oldValue.AuthenticationDisabled;
        Users = BuildConfiguration.Combine(oldValue.Users, newValue.Users);
    }

    public bool? AuthenticationDisabled { get; }
    public IEnumerable<GreenMailUser>? Users { get; }
}
