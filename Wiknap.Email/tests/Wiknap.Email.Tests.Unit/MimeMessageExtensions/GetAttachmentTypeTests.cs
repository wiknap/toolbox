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
            yield return [ContentType.Parse("message/rfc822"), EmailAttachmentType.Rfc822];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
