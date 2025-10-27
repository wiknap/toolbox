using Microsoft.Extensions.Logging;

namespace Wiknap.Email.DependencyInjection;

public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Sending email to {email}")]
    public static partial void SendingEmail(this ILogger logger, string email);

    [LoggerMessage(Level = LogLevel.Information, Message = "Email sent to {email}")]
    public static partial void EmailSent(this ILogger logger, string email);

    [LoggerMessage(Level = LogLevel.Information, Message = "Searching for email")]
    public static partial void SearchingEmail(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Email found")]
    public static partial void EmailFound(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Email not found")]
    public static partial void EmailNotFound(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Email is excluded {email}")]
    public static partial void EmailExcluded(this ILogger logger, string email);

    [LoggerMessage(Level = LogLevel.Information, Message = "Searching for emails")]
    public static partial void SearchingEmails(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Email found - count {count}")]
    public static partial void EmailsFound(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Emails not found")]
    public static partial void EmailsNotFound(this ILogger logger);
}
