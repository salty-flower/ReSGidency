using System.CommandLine;

namespace ReSGidency.MetaParser.PromptWriter
{
    static class Commands
    {
        private static readonly Option<IEnumerable<string>> RawEntriesOption =
            new(
                aliases: ["-e", "--entries"],
                description: "The raw entries to be processed.",
                getDefaultValue: () => []
            );

        private static readonly Option<FileInfo> OutputFileOption =
            new(
                aliases: ["-o", "--output-file"],
                description: "The file to write the prompt text to.",
                getDefaultValue: () => new("prompt.txt")
            );

        private static readonly Command processEntriesCommand =
            new("process-entries", "Process the raw entries into prompt text.")
            {
                RawEntriesOption,
                OutputFileOption
            };

        internal static Command ProcessEntriesCommand
        {
            get
            {
                processEntriesCommand.SetHandler(
                    PrintFullPrompt,
                    RawEntriesOption,
                    OutputFileOption
                );
                return processEntriesCommand;
            }
        }

        private static void PrintFullPrompt(IEnumerable<string> entries, FileInfo outputFile) =>
            File.WriteAllText(
                outputFile.FullName,
                $"{Configs.FULL_HEADER}\n{Configs.GetConcatenatedEntries(entries)}"
            );
    }
}
