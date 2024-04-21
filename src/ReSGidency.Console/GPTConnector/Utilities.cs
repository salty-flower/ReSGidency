using System.Collections.Concurrent;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ReSGidency.MetaParser.GPTConnector;

static class Utilities
{
    internal static async Task<IList<string>> ParseBatchAsync(
        IList<string> entries,
        OpenAIAPI apiInstance
    )
    {
        if (entries.Count > Configs.MAX_ENTRIES_ALLOWED)
        {
            throw new ArgumentOutOfRangeException(
                nameof(entries),
                $"Can only process <=${Configs.MAX_ENTRIES_ALLOWED} items at one time."
            );
        }

        var chatReq = new ChatRequest
        {
            Messages =
            [
                new(ChatMessageRole.System, PromptWriter.Configs.FULL_HEADER),
                .. entries.Select(str => new ChatMessage(ChatMessageRole.User,str) )
            ],
            Model = Model.GPT4_Turbo
        };
        return (await apiInstance.Chat.CreateChatCompletionAsync(chatReq))
            .Choices[0]
            .Message.TextContent.Split(['\n', '\r'])
            .Where(str => str != "")
            .ToList();
    }

    internal static async Task<string> ParseBulkAsync(IEnumerable<string> entries, string openAIKey)
    {
        OpenAIAPI apiInstance = new(openAIKey);

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
                    Console.WriteLine($"Requested {item.Length} entries.");
                    var thisBatchResult = (await ParseBatchAsync(item, apiInstance))
                        .Select(SanitizeGPTResponse)
                        .ToList();
                    Console.WriteLine($"Processed {thisBatchResult.Count} entries.");
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
        (await new OpenAI_API.OpenAIAPI(openAIKey).Chat.CreateChatCompletionAsync(fullPrompt))
            .Choices[0]
            .Message
            .TextContent;
}
