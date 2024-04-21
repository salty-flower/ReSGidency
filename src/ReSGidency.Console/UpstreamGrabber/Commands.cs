using System.CommandLine;

namespace ReSGidency.MetaParser.UpstreamGrabber;

static class Commands
{
    // output file option
    private static readonly Option<FileInfo?> OutputFileOption =
        new(
            aliases: ["-o", "--output-file"],
            description: "The file to write the response text to.",
            getDefaultValue: () => new("entries.txt")
        );

    // everything else
    private static readonly Command grabEntriesCommand =
        new("grab-entries", "Fetch and parse the upstream entries.") { OutputFileOption };

    internal static Command GrabEntriesCommand
    {
        get
        {
            grabEntriesCommand.SetHandler(ProcessEntriesAsync, OutputFileOption);
            return grabEntriesCommand;
        }
    }

    private static async Task ProcessEntriesAsync(FileInfo? outputFile)
    {
        var entries = await Utilities.FetchAndParseEntriesAsync();
        if (outputFile is not null)
        {
            File.WriteAllLines(outputFile.FullName, entries);
        }

        foreach (var entry in entries)
        {
            Console.WriteLine(entry);
        }
    }
}
