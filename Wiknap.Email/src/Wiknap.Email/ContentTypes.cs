using MimeKit;

namespace Wiknap.Email;

public static class ContentTypes
{
    public static ContentType Gif => ContentType.Parse(MimeTypes.Images.Gif);
    public static ContentType Png => ContentType.Parse(MimeTypes.Images.Png);
}
