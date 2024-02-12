using Microsoft.Extensions.Logging;

namespace Wiknap.Email.DependencyInjection;

public static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Sending email to {email}")]
    public static partial void SendingEmail(this ILogger logger, string email);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Email sent to {email}")]
    public static partial void EmailSent(this ILogger logger, string email);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Searching for email")]
    public static partial void SearchingEmail(this ILogger logger);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Email found")]
    public static partial void EmailFound(this ILogger logger);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Email not found")]
    public static partial void EmailNotFound(this ILogger logger);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information, Message = "Email is excluded {email}")]
    public static partial void EmailExcluded(this ILogger logger, string email);
}
