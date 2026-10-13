using Docker.DotNet.Models;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace Wiknap.Testcontainers.GreenMail;

public sealed class GreenMailBuilder : ContainerBuilder<GreenMailBuilder, GreenMailContainer, GreenMailConfiguration>
{
    private const string GreenMailImage = "greenmail/standalone:2.1.14";
    private const string GreenMailOptionsVariable = "GREENMAIL_OPTS";
    private const string GreenMailOptions =
        "-Dgreenmail.setup.test.all -Dgreenmail.hostname=0.0.0.0 " +
        "-Dgreenmail.tls.keystore.file=/home/greenmail/greenmail.p12 -Dgreenmail.tls.keystore.password=changeit";
    private const string AuthenticationDisabledOption = " -Dgreenmail.auth.disabled";

    public const ushort SmtpPort = 3025;
    public const ushort ImapPort = 3143;
    public const ushort Pop3Port = 3110;
    public const ushort ApiPort = 8080;

    public GreenMailBuilder()
        : this(new GreenMailConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }

    private GreenMailBuilder(GreenMailConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    protected override GreenMailConfiguration DockerResourceConfiguration { get; }

    public GreenMailBuilder WithAuthenticationDisabled(bool authenticationDisabled)
        => Merge(DockerResourceConfiguration, new GreenMailConfiguration(authenticationDisabled: authenticationDisabled))
            .WithEnvironment(GreenMailOptionsVariable,
                authenticationDisabled ? GreenMailOptions + AuthenticationDisabledOption : GreenMailOptions);

    public GreenMailBuilder WithUser(string email, string password)
        => Merge(DockerResourceConfiguration, new GreenMailConfiguration(users: [new GreenMailUser(email, password)]));

    public override GreenMailContainer Build()
    {
        Validate();

        var greenMailBuilder = DockerResourceConfiguration.WaitStrategies.Count() > 1
            ? this
            : WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(request => request
                    .ForPort(ApiPort)
                    .ForPath("/api/service/readiness")));
        return new GreenMailContainer(greenMailBuilder.DockerResourceConfiguration);
    }

    protected override GreenMailBuilder Init()
    {
        return base.Init()
            .WithImage(GreenMailImage)
            .WithPortBinding(SmtpPort, true)
            .WithPortBinding(ImapPort, true)
            .WithPortBinding(Pop3Port, true)
            .WithPortBinding(ApiPort, true)
            .WithAuthenticationDisabled(true);
    }

    protected override GreenMailBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        => Merge(DockerResourceConfiguration, new GreenMailConfiguration(resourceConfiguration));

    protected override GreenMailBuilder Clone(IContainerConfiguration resourceConfiguration)
        => Merge(DockerResourceConfiguration, new GreenMailConfiguration(resourceConfiguration));

    protected override GreenMailBuilder Merge(GreenMailConfiguration oldValue, GreenMailConfiguration newValue)
        => new(new GreenMailConfiguration(oldValue, newValue));
}
