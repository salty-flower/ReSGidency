using System.CommandLine;

namespace ReSGidency.MetaParser.End2endRunner;

static class Commands
{
    private static readonly Option<FileInfo?> JsonOutputOption =
        new(
            aliases: ["-j", "--json-output"],
            description: "The file to write the GPT-parsed JSON to.",
            getDefaultValue: () => new("gpt-parsed.json")
        );

    private static readonly Command runEnd2endCommand =
        new("run-end2end", "Run the whole process and generate GPT-parsed JSON.")
        {
            JsonOutputOption,
            GPTConnector.Commands.APIKeyOption
        };

    internal static Command RunEnd2endCommand
    {
        get
        {
            runEnd2endCommand.SetHandler(
                ProcessEnd2endAsync,
                JsonOutputOption,
                GPTConnector.Commands.APIKeyOption
            );
            return runEnd2endCommand;
        }
    }

    private static async Task ProcessEnd2endAsync(FileInfo? jsonOutput, string openAIKey)
    {
        var entries = await UpstreamGrabber.Utilities.FetchAndParseEntriesAsync();
        var gptParsedEntries = await GPTConnector.Utilities.ParseBulkAsync(entries, openAIKey);
        if (jsonOutput is not null)
        {
            await File.WriteAllLinesAsync(jsonOutput.FullName, gptParsedEntries);
        }
        foreach (var e in gptParsedEntries)
        {
            Console.WriteLine(e);
        }
    }
}
