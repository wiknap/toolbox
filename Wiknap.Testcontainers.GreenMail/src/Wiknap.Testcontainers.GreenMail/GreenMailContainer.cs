using System.Net.Http.Json;

using DotNet.Testcontainers.Containers;

namespace Wiknap.Testcontainers.GreenMail;

public sealed class GreenMailContainer : DockerContainer
{
    private readonly GreenMailConfiguration _configuration;

    public GreenMailContainer(GreenMailConfiguration configuration)
        : base(configuration)
    {
        _configuration = configuration;
    }

    public ushort SmtpPort => GetMappedPublicPort(GreenMailBuilder.SmtpPort);
    public ushort ImapPort => GetMappedPublicPort(GreenMailBuilder.ImapPort);
    public ushort Pop3Port => GetMappedPublicPort(GreenMailBuilder.Pop3Port);
    public ushort ApiPort => GetMappedPublicPort(GreenMailBuilder.ApiPort);
    public bool AuthenticationDisabled => _configuration.AuthenticationDisabled ?? true;
    public IReadOnlyCollection<GreenMailUser> Users => [.._configuration.Users ?? []];

    public Uri ApiAddress => new UriBuilder(Uri.UriSchemeHttp, Hostname, ApiPort).Uri;

    public override async Task StartAsync(CancellationToken ct = new())
    {
        await base.StartAsync(ct).ConfigureAwait(false);

        foreach (var user in Users)
            await AddUserAsync(user.Email, user.Password, ct).ConfigureAwait(false);
    }

    public async Task AddUserAsync(string email, string password, CancellationToken ct = default)
    {
        using var client = CreateApiClient();
        using var response = await client
            .PostAsJsonAsync(new Uri("/api/user", UriKind.Relative),
                new { email, login = email, password }, ct)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task PurgeAsync(CancellationToken ct = default)
    {
        using var client = CreateApiClient();
        using var response = await client
            .PostAsync(new Uri("/api/mail/purge", UriKind.Relative), null, ct)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private HttpClient CreateApiClient() => new() { BaseAddress = ApiAddress };
}
