using Zuhid.Messaging.Composers;

namespace Zuhid.Messaging.Tests.Composers;

public class EmailComposerTests
{
    // EmailComposer has protected methods and a private static property.
    // It's mostly a base class. We can test it through a derived class or by making a test-derived class.

    private class TestEmailComposer : BaseEmailComposer
    {
        public Task<string> PublicReadTemplate(string filePath) => ReadTemplate(filePath);
        public Task<string> PublicCreateHtmlAsync(string body, string style = "") => CreateHtmlAsync(body, style);
    }

    [Fact]
    public async Task CreateHtmlAsync_ReturnsExpectedHtml()
    {
        // This test might fail if Base.css is not in the expected location during test execution
        // AppContext.BaseDirectory in tests usually points to bin/Debug/net10.0

        var composer = new TestEmailComposer();
        var body = "<h1>Hello</h1>";
        var style = "h1 { color: red; }";

        // Act
        var result = await composer.PublicCreateHtmlAsync(body, style);

        // Assert
        Assert.Contains(body, result);
        Assert.Contains(style, result);
        Assert.Contains("<html>", result);
    }
}
