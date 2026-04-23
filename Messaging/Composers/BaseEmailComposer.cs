namespace Zuhid.Messaging.Composers;

public class BaseEmailComposer
{
    private static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "Templates");

    protected static async Task<string> ReadTemplate(string filePath)
    {
        return await File.ReadAllTextAsync(Path.Combine(TemplateDir, filePath));
    }

    protected static async Task<string> CreateHtmlAsync(string body, string style = "")
    {
        return $"""
<html>
<head>
<style>
    {await File.ReadAllTextAsync(Path.Combine(TemplateDir, "Base.css"))}
    {style}
</style>
</head>
<body>{body}</body>
</html>
""";
    }
}
