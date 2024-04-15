using OpenAI_API.Models;

namespace ReSGidency.MetaParser.GPTConnector;

static class Configs
{
    internal const string OPENAI_KEY_ENVIRONMENT_VARIABLE = "OPENAI_API_KEY";

    internal const int MAX_ENTRIES_ALLOWED = 13;

    internal const int MAX_REQUESTS_PER_MINUTE = 3;
}
