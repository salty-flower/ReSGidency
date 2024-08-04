using System.Diagnostics.CodeAnalysis;
using CliFx;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using ReSGidency.Clients;
using ReSGidency.Clients.Fetching;
using ReSGidency.CliFx.Commands;
using Serilog;
using Serilog.Core;

internal class Program
{
    private static readonly Logger logger = new LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Information()
        .CreateLogger();

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(GetIndustriesCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(GetApplicationRecordsCommand))]
    private static async Task Main() =>
        await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(static types =>
            {
                var services = new ServiceCollection();
                services.AddSingleton(
                    () =>
                        new ChatClient(
                            model: "gpt-4o-mini",
                            options: new OpenAI.OpenAIClientOptions { },
                            credential: new("")
                        )
                );
                services.AddTransient<HttpClient>();
                services.AddLogging(static builder => builder.AddSerilog(logger));
                services.AddSingleton<IndustryClient>();
                services.AddSingleton<ApplicationRecordsClient>();
                foreach (var type in types)
                    services.AddTransient(type);

                return services.BuildServiceProvider();
            })
            .SetExecutableName("ReSGidency")
            .Build()
            .RunAsync();
}
