using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ReSGidency.Clients.Fetching;
using ReSGidency.Models;

namespace ReSGidency.CliFx.Commands;

[Command(CommandName)]
public class GetApplicationRecordsCommand(ApplicationRecordsClient client) : ICommand
{
    public const string CommandName = "get-application-records";

    [CommandOption("write-to-console", 'c', Description = "Write the records to the console.")]
    public bool WriteToConsole { get; set; } = false;

    [CommandOption("output-path", 'o', Description = "The path to write the records to.")]
    public string OutputPath { get; set; } = "records.json";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var doc = await client.LoadFromRemoteAsync();
        var records = doc.Parse();

        if (WriteToConsole)
        {
            console.Output.WriteLine(string.Join('\n', records.ToString()));
        }

        await File.WriteAllTextAsync(
            OutputPath,
            JsonSerializer.Serialize(
                records,
                ApplicationRecord.JsonContext.Default.IEnumerableApplicationRecord
            )
        );

        return;
    }
}
