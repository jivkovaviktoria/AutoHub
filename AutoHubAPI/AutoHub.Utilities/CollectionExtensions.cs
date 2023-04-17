namespace AutoHub.Utilities;

public static class CollectionExtensions
{
    public static IEnumerable<TEntity> OrEmptyIfNull<TEntity>(this IEnumerable<TEntity>? collection)
    {
        if (collection is null) return Enumerable.Empty<TEntity>();
        return collection;
    }
}