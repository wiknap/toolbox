using System.Diagnostics;

using Bogus;

using Xunit;

namespace Wiknap.Email.Tests.Integration.Fixture;

[Collection("EmailServer")]
public abstract class IntegrationTestsBase : IDisposable
{
    protected readonly IEmailClient EmailClient;
    private readonly IEmailClient userEmailClient;
    protected const string UserEmail = EmailServer.UserEmail;
    protected readonly Faker Faker = new();
    protected readonly CancellationTokenSource Cts = new();

    protected IntegrationTestsBase(EmailServer emailServer)
    {
        var config = new TestEmailClientConfiguration(EmailServer.Host, emailServer.SmtpPort, EmailServer.Host,
            emailServer.ImapPort, emailServer.AdminEmail, emailServer.AdminPassword);
        EmailClient = new Email.EmailClient(config);
        var userConfig = new TestEmailClientConfiguration(EmailServer.Host, emailServer.SmtpPort, EmailServer.Host,
            emailServer.ImapPort, EmailServer.UserEmail, EmailServer.UserPassword);
        userEmailClient = new Email.EmailClient(userConfig);
    }

    protected async Task<string?> GetUserEmailContentAsync(SearchParameters searchParameters,
        EmailContentType? emailContentType = null)
    {
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < TimeSpan.FromSeconds(3))
        {
            var content = await userEmailClient
                .GetEmailContentAsync(
                    new SearchParameters
                    {
                        SenderEmail = searchParameters.SenderEmail, Subject = searchParameters.Subject
                    }, emailContentType, Cts.Token).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(content))
                return content;
        }

        return null;
    }

    public void Dispose() => Cts.Dispose();
}
