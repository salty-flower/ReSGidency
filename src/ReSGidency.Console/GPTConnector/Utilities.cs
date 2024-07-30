using System.Collections.Concurrent;
using OpenAI.Chat;

namespace ReSGidency.Console.GPTConnector;

static class Utilities
{
    internal static async Task<IList<string>> ParseBatchAsync(
        IList<string> entries,
        ChatClient apiInstance
    )
    {
        if (entries.Count > Configs.MAX_ENTRIES_ALLOWED)
        {
            throw new ArgumentOutOfRangeException(
                nameof(entries),
                $"Can only process <=${Configs.MAX_ENTRIES_ALLOWED} items at one time."
            );
        }

        var messages = (ChatMessage[])

            [
                ChatMessage.CreateSystemMessage(PromptWriter.Configs.FULL_HEADER),
                .. entries.Select(str => ChatMessage.CreateUserMessage(str))
            ];

        return (await apiInstance.CompleteChatAsync(messages))
            .Value.Content.Select(part => part.Text)
            .ToList();
    }

    internal static async Task<string> ParseBulkAsync(IEnumerable<string> entries, string openAIKey)
    {
        ChatClient apiInstance =
            new(
                model: "gpt-4o-mini",
                credential: new System.ClientModel.ApiKeyCredential(openAIKey)
            );

        var result = new List<string>();
        var tasks = new ConcurrentBag<Task>();

        var groups = entries
            .Chunk(Configs.MAX_ENTRIES_ALLOWED)
            .Chunk(Configs.MAX_REQUESTS_PER_MINUTE)
            .Select(g => g.ToList())
            .ToList();

        foreach (var g in groups)
        {
            foreach (
                var t in g.Select(async item =>
                {
                    System.Console.WriteLine($"Requested {item.Length} entries.");
                    var thisBatchResult = (await ParseBatchAsync(item, apiInstance))
                        .Select(SanitizeGPTResponse)
                        .ToList();
                    System.Console.WriteLine($"Processed {thisBatchResult.Count} entries.");
                    lock (result)
                        result.AddRange(thisBatchResult);
                })
            )
                tasks.Add(t);

            if (g != groups.Last())
                await Task.Delay(TimeSpan.FromMinutes(1));
        }
        await Task.WhenAll(tasks);
        return $"[{string.Join(',', result)}]";
    }

    internal static string SanitizeGPTResponse(string response)
    {
        if (response.Last() == ',')
            response = response[..^1];

        return response.Replace("```json", "").Replace("```", "");
    }

    internal static async Task<string> GetGPTReply(string fullPrompt, string openAIKey) =>
        string.Join(
            '\n',
            (
                await new ChatClient(
                    model: "gpt-4o-mini",
                    credential: new System.ClientModel.ApiKeyCredential(openAIKey)
                ).CompleteChatAsync(fullPrompt)
            )
                .Value
                .Content
        );
}
