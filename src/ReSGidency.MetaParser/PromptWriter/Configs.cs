using ReSGidency.Models;

namespace ReSGidency.MetaParser.PromptWriter;

static partial class Configs
{
    internal const string PARSE_HINTS = """
        1. Nationality is inferrable from context. Chinese text might from China/Malaysia. "国内" in most case is China.s
        2. Entry format is "Username Descriptions Status ApplicationDate EndDate? UpdateDate"
        """;

    internal const string HEADER_TEXT = $"""
        Hi. I would like you to help me extract information from unstructured text.
        Specifically, these are info shared by individuals
        about their permanent residency/citizenship application in Singapore.
        Note that
            {PARSE_HINTS}

        I will supply you the full definition for each entity with C# class code,
        and the list of industries from the Singapore Standard Industrial Classification (SSIC).
        You should output the respetive JSON data only, with no other text.
        """;

    internal const string DEFINITION_TEXT =
        $"===BEGIN DEF\n{ConstantsContainer.Definitions}\n===END DEF";

    internal const string FULL_HEADER = $"{HEADER_TEXT}\n{DEFINITION_TEXT}\n{INDUSTRIES_TEXT}";

    internal static string GetConcatenatedEntries(IEnumerable<string> entries) =>
        $"===BEGIN ENTRIES\n{string.Join("\n\n", entries)}\n===END ENTRIES";
}
