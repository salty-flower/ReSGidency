using OpenAI.Chat;

namespace ReSGidency.Clients.Parsing;

public interface IParseClient<TUnstructured, TExtracted, TStreamedPiece>
{
    event EventHandler<TStreamedPiece>? OnPieceArrived;
    IList<TUnstructured> UnstructuredData { get; init; }
    IList<ChatMessage> PromptSet { get; }
    Task<IList<TExtracted>> ParseAsync();
}
