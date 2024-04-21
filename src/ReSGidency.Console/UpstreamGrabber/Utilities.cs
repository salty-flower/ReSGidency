using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ReSGidency.MetaParser.UpstreamGrabber;

static partial class Utilities
{
    internal static string[] ParseEntries(HtmlDocument htmlDocument) =>
        htmlDocument
            .DocumentNode.SelectSingleNode("//table/tbody")
            .ChildNodes.Where(n => n.Name == "tr")
            .Select(item =>
                item.ChildNodes.Where(n => n.Name == "td")
                    .Skip(1)
                    .Select(x => LinebreakRegex().Replace(x.InnerText.Trim(), "\\n"))
                    .Where(s => s != "")
                    .Aggregate((a, b) => $"{a} || {b}")
            )
            .ToArray();

    internal static async Task<string[]> FetchAndParseEntriesAsync() =>
        ParseEntries(await new HtmlWeb().LoadFromWebAsync(Configs.LISTING_URL));

    [GeneratedRegex(@"[\r\n]")]
    private static partial Regex LinebreakRegex();
}
