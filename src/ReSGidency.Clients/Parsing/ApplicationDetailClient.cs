using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using ReSGidency.Models;

namespace ReSGidency.Clients.Parsing;

public class ApplicationDetailClient(ChatClient chatClient, ILogger<ApplicationDetailClient> logger)
    : IParseClient<string, ApplicationDetail, StreamingChatCompletionUpdate>
{
    public record struct ClientMetadata(Industry[] Industries, string DefinitionCode);

    public ClientMetadata? Metadata { get; init; }

    private const string SystemPrompt = $"""
        You are a former ICA officer, now working as a data analyst,
        specialized in extracting information from unstructured text.
        You are capable to examine details shared by individuals
        and align their situation with preset fields/attributes.

        You will be given unstructured string and are required to
        respond with {nameof(ApplicationDetail)} objects in JSON.
        The object specification and industries are as follows:
        """;

    private static ApplicationDetail example1 =
        new()
        {
            Level = InstitutionDescriptors.Level.NTU_NUS,
            Nationality = "CHN",
            BaseMonthSalary = 5000,
            DurationInSG = TimeSpan.FromDays(1840),
            FormerFailedApplicationCount = 0,
            Industry =
                "computer programming, information technology consultancy and related activities",
            ModifierFlag = ApplicationModifiers.None,
            PermitHistory = [Permit.EP, Permit.STP],
            Qualifications =
            [
                new Qualification(
                    QualificationLevel.Bachelor,
                    ["NUS"],
                    "National University of Singapore",
                    "Computer Science",
                    new DateOnly(2018, 07, 10),
                    "Singapore"
                )
            ],
        };

    private const string example1string = """
        2018年7月NUS计算机本科毕业，中国国籍，起薪5000新币，IT行业工作一年后拿到，
        第一次申请，望好运。
        """;

    public event EventHandler<StreamingChatCompletionUpdate>? OnPieceArrived;

    public required IList<string> UnstructuredData { get; init; }
    public IList<ChatMessage> PromptSet =>
        [
            new SystemChatMessage(
                SystemPrompt
                    + (
                        Metadata.HasValue
                            ? new SystemChatMessage(
                                string.Join('\n', Metadata.Value.Industries.Select(i => i.Name))
                                    + "\n\n```csharp"
                                    + Metadata.Value.DefinitionCode
                                    + "```"
                            )
                            : ""
                    )
            ),
            new UserChatMessage(example1string),
            new AssistantChatMessage(JsonSerializer.Serialize((ApplicationDetail[])[example1])),
            new UserChatMessage(string.Join("\n\n", UnstructuredData)),
        ];

    public async Task<IList<ApplicationDetail>> ParseAsync()
    {
        logger.BeginScope("Parsing Application Details");
        var msgBuffer = new StringBuilder();

        await foreach (var message in chatClient.CompleteChatStreamingAsync(PromptSet))
        {
            logger.LogTrace(
                "Received message: created at {createdAt}, {count} pieces",
                message.CreatedAt,
                message.ContentUpdate.Count
            );
            OnPieceArrived?.Invoke(this, message);
            msgBuffer.Append(message.ContentUpdate.Select(u => u.Text));
        }

        logger.LogInformation("Parsing completed with {length} characters", msgBuffer.Length);
        return JsonSerializer.Deserialize<IList<ApplicationDetail>>(msgBuffer.ToString())!;
    }
}
