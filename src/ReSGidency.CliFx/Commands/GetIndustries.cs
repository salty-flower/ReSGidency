using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Logging;
using ReSGidency.Clients;
using Spectre.Console;

namespace ReSGidency.CliFx.Commands;

[Command(CommandName)]
public class GetIndustriesCommand(IndustryClient client, ILogger<GetIndustriesCommand> logger)
    : ICommand
{
    public const string CommandName = "get-industries";

    [CommandOption("write-to-console", 'c', Description = "Write the records to the console.")]
    public bool WriteToConsole { get; set; } = false;

    [CommandOption(
        "save-path",
        'p',
        Description = "The path to save the industries to. Defaults to 'industries.txt'"
    )]
    public string OutputPath { get; set; } = "industries.txt";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var doc = await client.LoadFromRemoteAsync();
        var industries = doc.Parse()!;
        logger.LogInformation("Found {Count} industries", industries.Count);
        File.WriteAllLines(OutputPath, industries.Select(i => i.Name));
        return;
    }
}
