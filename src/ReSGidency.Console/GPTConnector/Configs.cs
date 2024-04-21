using OpenAI_API.Models;
﻿using System.Text.RegularExpressions;

namespace ReSGidency.MetaParser.GPTConnector;

static partial class Configs
{
    internal const string OPENAI_KEY_ENVIRONMENT_VARIABLE = "OPENAI_API_KEY";

    internal const int MAX_ENTRIES_ALLOWED = 13;

    internal const int MAX_REQUESTS_PER_MINUTE = 3;

    [GeneratedRegex(@"sk-[0-9a-zA-Z]{20}T3BlbkFJ[0-9a-zA-Z]{20}")]
    internal static partial Regex OpenAIAPIKeyPattern();
}
