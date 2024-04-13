using System.CommandLine;

namespace ReSGidency.MetaParser;

public class Program
{
    private const string Description =
        "ReSGidency.MetaParser - " + "A tool to parse metadata for the ReSGidency project.";

    static readonly RootCommand EntryCommand = new(description: Description);

    static async Task<int> Main(string[] args)
    {
        EntryCommand.AddCommand(IndustryParser.Commands.GetIndustriesCommand);
        return await EntryCommand.InvokeAsync(args);
    }
}
