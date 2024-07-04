namespace Wiknap.Email;

public static class MimeTypes
{
    public static class Application
    {
        public const string Csv = "text/csv";
        public const string Doc = "application/msword";
        public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string Pdf = "application/pdf";
        public const string Xls = "application/vnd.ms-excel";
        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Zip = "application/zip";
        public const string ZipWindows = "application/x-zip-compressed";
    }

    public static class Images
    {
        public const string Gif = "image/gif";
        public const string Png = "image/png";
        public const string Jpeg = "image/jpeg";
        public const string Svg = "image/svg+xml";
        public const string Bmp = "image/bmp";
    }

    public static class Message
    {
        public const string Rfc822 = "message/rfc822";
    }
}
