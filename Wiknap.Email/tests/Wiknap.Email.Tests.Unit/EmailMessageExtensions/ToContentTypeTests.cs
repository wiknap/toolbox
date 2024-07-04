using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.EmailMessageExtensions;

public sealed class ToContentTypeTests : TestsBase
{
    [Theory]
    [InlineData(EmailAttachmentType.Png, "image/png")]
    [InlineData(EmailAttachmentType.Gif, "image/gif")]
    [InlineData(EmailAttachmentType.Jpeg, "image/jpeg")]
    [InlineData(EmailAttachmentType.Svg, "image/svg+xml")]
    [InlineData(EmailAttachmentType.Bmp, "image/bmp")]
    [InlineData(EmailAttachmentType.Csv, "text/csv")]
    [InlineData(EmailAttachmentType.Doc, "application/msword")]
    [InlineData(EmailAttachmentType.Docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData(EmailAttachmentType.Pdf, "application/pdf")]
    [InlineData(EmailAttachmentType.Xls, "application/vnd.ms-excel")]
    [InlineData(EmailAttachmentType.Xlsx, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [InlineData(EmailAttachmentType.Zip, "application/zip")]
    [InlineData(EmailAttachmentType.ZipWindows, "application/x-zip-compressed")]
    [InlineData(EmailAttachmentType.Rfc822, "image/gif")]
    public void Given_EmailAttachmentType_When_ToContentType_Then_ReturnContentTypeWityMatchingMimeType(EmailAttachmentType type, string expectedType)
    {
        // Act
        var contentType = type.ToContentType();

        // Assert
        contentType.MimeType.ShouldBe(expectedType);
    }
}
