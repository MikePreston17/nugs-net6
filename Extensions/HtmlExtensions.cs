using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

// using nugsnet6;
namespace CodeMechanic.Advanced.Extensions;

/// CREDIT: https://www.adamrussell.com/asp-net-core-section-scripts-in-a-partial-view

public static class HtmlExtensions
{
    /// <summary>
    /// Quick wrapper for creating safe HTML strings on the backend.
    /// </summary>
    public static HtmlString AsHTMLString(this string raw_text)
    {
        var clone = new StringBuilder(raw_text); // lazy clone.
        var result = new HtmlString(clone.ToString());
        return result;
    }

    private const string _partialViewScriptItemPrefix = "scripts_";

    public static IHtmlContent PartialSectionScripts(
        this IHtmlHelper htmlHelper,
        Func<object, HelperResult> template
    )
    {
        htmlHelper.ViewContext.HttpContext.Items[_partialViewScriptItemPrefix + Guid.NewGuid()] =
            template;
        return new HtmlContentBuilder();
    }

    public static IHtmlContent RenderPartialSectionScripts(this IHtmlHelper htmlHelper)
    {
        var partialSectionScripts = htmlHelper.ViewContext.HttpContext.Items.Keys.Where(k =>
            System.Text.RegularExpressions.Regex.IsMatch(
                k.ToString(),
                "^"
                    + _partialViewScriptItemPrefix
                    + "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$"
            )
        );
        var contentBuilder = new HtmlContentBuilder();
        foreach (var key in partialSectionScripts)
        {
            var template =
                htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
            if (template != null)
            {
                var writer = new System.IO.StringWriter();
                template(null).WriteTo(writer, HtmlEncoder.Default);
                contentBuilder.AppendHtml(writer.ToString());
            }
        }
        return contentBuilder;
    }
}
