using System.Data;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace ReSGidency.MetaParser.IndustryParser;

static partial class Utilities
{
    internal static async Task DownloadTableAsync(string path)
    {
        if (File.Exists(path))
            return;

        using FileStream file = new(path, FileMode.Create);
        using HttpClient client = new();
        (await client.GetAsync(Configs.TableURL)).Content.ReadAsStream().CopyTo(file);
    }

    internal static string[] GetIndustries(Stream stream)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var industries = new List<string>(20);

        foreach (DataRow row in reader.AsDataSet().Tables[0].Rows)
        {
            if (
                row.ItemArray.Length == 0
                || row.ItemArray[0] == null
                || !Level1IndustryPattern().IsMatch(row.ItemArray[0]!.ToString()!)
                || row.ItemArray[1] == null
            )
                continue;
            industries.Add(row.ItemArray[1]!.ToString()!.ToLower());
        }

        return [.. industries];
    }

    [GeneratedRegex(@"^\d{2}$")]
    private static partial Regex Level1IndustryPattern();
}
