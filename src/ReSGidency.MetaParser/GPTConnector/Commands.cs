using System.CommandLine;

namespace ReSGidency.MetaParser.GPTConnector;

static class Commands
{
    private static readonly Option<FileInfo> PromptFileOption =
        new(
            aliases: ["-p", "--prompt-file"],
            description: "The file to read the prompt text from.",
            getDefaultValue: () => new("prompt.txt")
        );

    private static readonly Option<string> APIKeyOption =
        new(
            aliases: ["-k", "--openai-key"],
            description: $"The OpenAI API key to use, "
                + $"defaults to ${Configs.OPENAI_KEY_ENVIRONMENT_VARIABLE}.",
            getDefaultValue: () =>
                new(
                    Environment.GetEnvironmentVariable(Configs.OPENAI_KEY_ENVIRONMENT_VARIABLE)
                        ?? ""
                )
        );

    private static readonly Option<FileInfo?> OutputFileOption =
        new(
            aliases: ["-o", "--output-file"],
            description: "The file to write the response text to."
        );

    private static readonly Command generateResponseCommand =
        new("generate-response", "Generate a response from the prompt text.")
        {
            PromptFileOption,
            APIKeyOption,
            OutputFileOption
        };

    internal static Command GenerateResponseCommand
    {
        get
        {
            generateResponseCommand.SetHandler(
                GenerateResponseAsync,
                PromptFileOption,
                APIKeyOption,
                OutputFileOption
            );
            return generateResponseCommand;
        }
    }

    private static async Task GenerateResponseAsync(
        FileInfo promptFile,
        string openAIKey,
        FileInfo? outputFile
    )
    {
        var promptText = File.ReadAllText(promptFile.FullName);
        var api = new OpenAI_API.OpenAIAPI(openAIKey);
        var res = await api.Chat.CreateChatCompletionAsync(promptText);
        var responseText = res.Choices[0].Message.TextContent;

        if (outputFile is not null)
        {
            File.WriteAllText(outputFile.FullName, responseText);
        }
        Console.WriteLine(responseText);
    }
}
