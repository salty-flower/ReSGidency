using ReSGidency.Models;

namespace ReSGidency.MetaParser.PromptWriter;

static partial class Configs
{
    internal const string PARSE_HINTS = """
        - Entry format is "Username || Descriptions || Status || ApplicationDate || (EndDate ||)? UpdateDate"
        - Whenever the attribute is not present, give null, instead of arbitrary values.
        """;

    internal const string HEADER_TEXT = $"""
        You are expert in extracting information from unstructured text.
        Specifically, these are info shared by individuals
        about their permanent residency/citizenship application in Singapore.
        {PARSE_HINTS}

        You are given full definition for each entity with C# class code (use full name for enum, not integer),
        and the list of industries from the Singapore Standard Industrial Classification (SSIC).
        For each entry, you should write one JSON object in one line,
        with no other text, not even Markdown raw-text markers "```", and no surrounding array symbols "[]".
        """;

    internal const string DEFINITION_TEXT =
        $"===BEGIN DEF\n{ConstantsContainer.Definitions}\n===END DEF";

    internal const string FULL_HEADER = $"{HEADER_TEXT}\n{DEFINITION_TEXT}\n{INDUSTRIES_TEXT}";

    internal static string GetConcatenatedEntries(IEnumerable<string> entries) =>
        $"===BEGIN ENTRIES\n{string.Join("\n\n", entries)}\n===END ENTRIES";

    internal static string GetPromptText(IEnumerable<string> entries) =>
        $"{FULL_HEADER}\n{GetConcatenatedEntries(entries)}";
}
