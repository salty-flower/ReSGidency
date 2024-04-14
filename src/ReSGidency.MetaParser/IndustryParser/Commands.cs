using System.CommandLine;

namespace ReSGidency.MetaParser.IndustryParser;

static class Commands
{
    private static readonly Option<FileInfo> DownloadSSICPathOption =
        new(
            aliases: ["-d", "--download-path"],
            description: "The file to write the SSIC table to.",
            getDefaultValue: () => new FileInfo("SSIC.xlsx")
        );

    private static readonly Option<FileInfo> ParsedIndustryListPathOption =
        new(
            aliases: ["-o", "--parsed-industry-list-path"],
            description: "The file to write the parsed industry list to.",
            getDefaultValue: () => new FileInfo("industries.txt")
        );

    private static readonly Command getIndustriesCommand =
        new(
            "get-industries",
            "Get the list of industries from the Singapore Standard Industrial Classification (SSIC)."
        )
        {
            DownloadSSICPathOption,
            ParsedIndustryListPathOption
        };

    internal static Command GetIndustriesCommand
    {
        get
        {
            getIndustriesCommand.SetHandler(
                InvokeIndustryCommandAsync,
                DownloadSSICPathOption,
                ParsedIndustryListPathOption
            );
            return getIndustriesCommand;
        }
    }

    private static async Task InvokeIndustryCommandAsync(FileInfo downloadPath, FileInfo outputPath)
    {
        await Utilities.DownloadTableAsync(downloadPath.FullName);
        using FileStream file = new(downloadPath.FullName, FileMode.Open);
        await File.WriteAllLinesAsync(outputPath.FullName, Utilities.GetIndustries(file));
    }
}
