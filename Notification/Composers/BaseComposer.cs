namespace Zuhid.Notification.Composers;

public class BaseComposer
{
    private static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "Composers");

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
{await File.ReadAllTextAsync(Path.Combine(TemplateDir, "BaseComposer.css"))}{style}
</style>
</head>
<body>{body}</body>
</html>
""";
    }
}
