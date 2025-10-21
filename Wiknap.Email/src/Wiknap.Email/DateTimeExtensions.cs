namespace Wiknap.Email;

internal static class DateTimeExtensions
{
    public static DateTime TrimMilliseconds(this DateTime dt) =>
        new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);

    public static DateTimeOffset TrimMilliseconds(this DateTimeOffset dto) =>
        new(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second, 0, dto.Offset);
}
