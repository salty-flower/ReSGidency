namespace ReSGidency.Clients.Fetching;

public interface IListClient<T, TRaw>
{
    Task<IListDocument<T, TRaw>> LoadFromRemoteAsync();
}

public interface IListDocument<T, TRaw>
{
    TRaw RawData { get; init; }
    IReadOnlyList<T> Parse();
}
