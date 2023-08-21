using Microsoft.Azure.Cosmos;
using System.Runtime.CompilerServices;

namespace Shared.Extensions;

public static class CosmosLinqExtensions
{
    /// <summary>
    /// Convert a feed iterator to IAsyncEnumerable
    /// https://stackoverflow.com/questions/67317049/how-to-convert-cosmosdb-feediterator-result-to-iasyncenumerable-or-ienumerable/67317050#67317050
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="setIterator"></param>
    /// <returns></returns>

    public static async Task<List<TModel>> ToListAsync<TModel>(
        this FeedIterator<TModel> items,
        CancellationToken cancellationToken = default)
    {
        return await items
            .ToAsyncEnumerable(cancellationToken)
            .ToListAsyncInternal(cancellationToken)
            .ConfigureAwait(false);
    }
    private static async IAsyncEnumerable<TModel> ToAsyncEnumerable<TModel>(
        this FeedIterator<TModel> setIterator,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (setIterator.HasMoreResults)
            foreach (var item in await setIterator.ReadNextAsync(cancellationToken))
            {
                yield return item;
            }
    }

    private static async Task<List<T>> ToListAsyncInternal<T>(this IAsyncEnumerable<T> items,
        CancellationToken cancellationToken = default)
    {
        var results = new List<T>();
        await foreach (var item in items.WithCancellation(cancellationToken)
                                        .ConfigureAwait(false))
        {
            results.Add(item);
        }
            
        return results;
    }

}
