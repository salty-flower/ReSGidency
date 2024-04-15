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

    internal static async Task<IList<string>> ParseBulkAsync(
        IEnumerable<string> entries,
        string openAIKey
    )
    {
        OpenAIAPI apiInstance = new OpenAIAPI(openAIKey);

        var result = new List<string>();

        var groups = entries
            .Chunk(Configs.MAX_ENTRIES_ALLOWED)
            .Chunk(Configs.MAX_REQUESTS_PER_MINUTE)
            .ToList();

        foreach (var g in groups)
        {
            _ = Task.Run(() =>
            {
                foreach (var item in g)
                {
                    Console.WriteLine("requested!");
                    _ = Task.Run(() =>
                    {
                        result.AddRange(ParseBatchAsync(item, apiInstance).Result);
                    });
                }
            });
            await Task.Delay(TimeSpan.FromMinutes(1));
        }

        return result;
    }

    internal static async Task<string> GetGPTReply(string fullPrompt, string openAIKey) =>
        (await new OpenAI_API.OpenAIAPI(openAIKey).Chat.CreateChatCompletionAsync(fullPrompt))
            .Choices[0]
            .Message
            .TextContent;
}
