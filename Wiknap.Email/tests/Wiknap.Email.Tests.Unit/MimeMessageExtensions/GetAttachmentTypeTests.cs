using System.Collections;

using MimeKit;

using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.MimeMessageExtensions;

public sealed class GetAttachmentTypeTests : TestsBase
{
    [Theory]
    [ClassData(typeof(GetAttachmentTypeTestsData))]
    public void Given_ContentType_When_GetAttachmentType_Then_ReturnsEmailAttachmentType(ContentType contentType,
        EmailAttachmentType expectedType)
    {
        // Act
        var type = contentType.GetAttachmentType();

        // Assert
        type.ShouldBe(expectedType);
    }

    private sealed class GetAttachmentTypeTestsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [ContentType.Parse("image/gif"), EmailAttachmentType.Gif];
            yield return [ContentType.Parse("image/png"), EmailAttachmentType.Png];
            yield return [ContentType.Parse("image/jpeg"), EmailAttachmentType.Jpeg];
            yield return [ContentType.Parse("image/svg+xml"), EmailAttachmentType.Svg];
            yield return [ContentType.Parse("image/bmp"), EmailAttachmentType.Bmp];
            yield return [ContentType.Parse("text/csv"), EmailAttachmentType.Csv];
            yield return [ContentType.Parse("application/msword"), EmailAttachmentType.Doc];
            yield return [ContentType.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document"), EmailAttachmentType.Docx];
            yield return [ContentType.Parse("application/pdf"), EmailAttachmentType.Pdf];
            yield return [ContentType.Parse("application/vnd.ms-excel"), EmailAttachmentType.Xls];
            yield return [ContentType.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"), EmailAttachmentType.Xlsx];
            yield return [ContentType.Parse("application/zip"), EmailAttachmentType.Zip];
            yield return [ContentType.Parse("application/x-zip-compressed"), EmailAttachmentType.ZipWindows];
            yield return [ContentType.Parse("message/rfc822"), EmailAttachmentType.Rfc822];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
