using System.Collections.Immutable;
using System.Data;
using System.Text.RegularExpressions;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using ReSGidency.Clients.Fetching;

namespace ReSGidency.Clients;

public record struct Industry(string Name);

public partial class IndustryDocument : IListDocument<Industry, IExcelDataReader>
{
    [GeneratedRegex(@"^\d{2}$")]
    private static partial Regex Level1IndustryPattern();

    public required IExcelDataReader RawData { get; init; }

    public IReadOnlyList<Industry> Parse() =>
        (
            from DataRow row in RawData.AsDataSet().Tables[0].Rows
            where row.ItemArray.Length > 0
            where row.ItemArray[0] is not null
            where Level1IndustryPattern().IsMatch(row.ItemArray[0]!.ToString()!)
            where row.ItemArray[1] is not null
            select new Industry(Name: row.ItemArray[1]!.ToString()!.ToLower())
        ).ToImmutableArray();
}

public class IndustryClient(HttpClient client, ILogger<IndustryClient> logger)
    : IListClient<Industry, IExcelDataReader>
{
    private const string TableURL =
        "https://www.singstat.gov.sg/-/media/files/standards_and_classifications/"
        + "industrial_classification/ssic2020-classification-structure.ashx";

    public async Task<IListDocument<Industry, IExcelDataReader>> LoadFromRemoteAsync()
    {
        logger.LogInformation("Downloading SSIC table from {TableURL}", TableURL);
        var response = (await client.GetAsync(TableURL))!;
        logger.LogInformation(
            "Downloaded {Length} bytes, reported last modification {LastModified}",
            response.Content.Headers.ContentLength,
            response.Content.Headers.LastModified
        );
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        return new IndustryDocument
        {
            RawData = ExcelReaderFactory.CreateReader(response.Content.ReadAsStream())
        };
    }
}
