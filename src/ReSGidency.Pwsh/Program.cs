using System.Management.Automation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReSGidency.Clients;
using Serilog;

namespace ReSGidency.Pwsh;

[Cmdlet(VerbsCommon.Get, "Industries")]
public class GetIndustriesCommand : Cmdlet
{
    [Parameter(Mandatory = true, HelpMessage = "The file to write the SSIC table to.")]
    public string DownloadPath { get; set; } = "industries.txt";
    private IServiceProvider? serviceProvider;
    private static readonly Serilog.Core.Logger serilogLogger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();

    protected override void BeginProcessing()
    {
        var builder = new ServiceCollection();
        builder.AddLogging(static b => b.AddSerilog(serilogLogger));
        builder.AddSingleton<IndustryClient>();
        builder.AddSingleton<HttpClient>();
        serviceProvider = builder.BuildServiceProvider();
    }

    protected override void ProcessRecord()
    {
        var logger = serviceProvider!.GetRequiredService<ILogger<GetIndustriesCommand>>();
        logger.LogInformation("Downloading industries...");
        var doc = serviceProvider!
            .GetRequiredService<IndustryClient>()
            .LoadFromRemoteAsync()
            .Result;
        logger.LogInformation("Downloaded industries!");
        var industries = doc.Parse();
        logger.LogInformation("Parsed {count} industries", industries.Count);
        File.WriteAllLines(DownloadPath, industries.Select(static i => i.Name));
    }
}
