using System.Collections.Immutable;
using HtmlAgilityPack;
using ReSGidency.Models;

namespace ReSGidency.Clients.Fetching;

public class ApplicationRecordsDocument : IListDocument<ApplicationRecord, HtmlDocument>
{
    public required HtmlDocument RawData { get; init; }

    private static DateOnly? TryGetDate(string maybeDate) =>
        maybeDate.Trim().Length == 0
            ? null
            : DateOnly.TryParse(maybeDate, out var parsedDate)
                ? parsedDate
                : null;

    public IReadOnlyList<ApplicationRecord> Parse() =>
        RawData
            .DocumentNode.SelectSingleNode("//table/tbody")
            .ChildNodes.Where(n => n.Name == "tr")
            .Select(item =>
            {
                var cells = item.ChildNodes.Where(n => n.Name == "td").ToArray();
                return new ApplicationRecord(
                    cells[1].InnerText.Trim(),
                    cells[2].InnerText.Trim(),
                    cells[3].InnerText.Trim() switch
                    {
                        "等待" => ApplicationStatus.Pending,
                        "通过" => ApplicationStatus.Successful,
                        "杯具" => ApplicationStatus.Failed,
                        _ => ApplicationStatus.Unspecified
                    },
                    TryGetDate(cells[4].InnerText),
                    TryGetDate(cells[5].InnerText),
                    DateTime.Parse(cells[6].InnerText.Trim())
                );
            })
            .ToImmutableArray();
}

public class ApplicationRecordsClient : IListClient<ApplicationRecord, HtmlDocument>
{
    private const string ListingURL = "http://sgprapp.com/listPage";

    public async Task<IListDocument<ApplicationRecord, HtmlDocument>> LoadFromRemoteAsync() =>
        new ApplicationRecordsDocument
        {
            RawData = await new HtmlWeb().LoadFromWebAsync(ListingURL)
        };
}
