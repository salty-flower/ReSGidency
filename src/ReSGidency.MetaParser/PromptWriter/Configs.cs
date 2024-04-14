using ReSGidency.MetaParser.PromptWriter;
using ReSGidency.Models;

namespace ReSGidency.MetaParser.PromptWriter;

static class Configs
{
    internal const string HEADER_TEXT = """
        Hi. I would like you to help me extract information from unstructured text.
        Specifically, these are info shared by individuals
        about their permanent residency/citizenship application in Singapore.

        I will supply you the full definition for each entity with C# class code,
        and the list of industries from the Singapore Standard Industrial Classification (SSIC).
        You should output the respetive JSON data only, with no other text.
        """;

    internal const string DEFINITION_TEXT =
        $"===BEGIN DEF\n{ConstantsContainer.Definitions}\n===END DEF";

    internal static string GetConcatenatedIndustries(IEnumerable<string> industries) =>
        $"===BEGIN INDUSTRIES\n{string.Join('\n', industries)}\n===END INDUSTRIES";
    internal static string GetConcatenatedEntries(IEnumerable<string> entries) =>
        $"===BEGIN ENTRIES\n{string.Join("\n\n", entries)}\n===END ENTRIES";
}
