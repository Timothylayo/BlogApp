using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlogApp
{
    public static class MarkDownConversion
    {
        public static MarkupString ConvertMarkdownToHtml(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new MarkupString(string.Empty);
            }

            // Convert Markdown to HTML
            var html = Markdown.ToHtml(content);
            return new MarkupString(html);
        }
    }
}
