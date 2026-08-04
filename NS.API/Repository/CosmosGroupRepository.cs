using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using NS.API.Repository.Core;
using NS.Shared.Core.Types;
using System.Globalization;
using System.Linq.Expressions;

namespace NS.API.Repository;

public class CosmosGroupRepository(CosmosClient CosmosClient, ILogger<CosmosGroupRepository> logger)
    : BaseRepository<CosmosGroupRepository, GroupDocument, GroupIdentity>(CosmosClient, logger, "group")
{
    public async Task<IReadOnlyCollection<T>> Query<T>(GroupType type, Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IQueryable<T>>? transform, CancellationToken cancellationToken)
        where T : GroupDocument
    {
        try
        {
            var queryable = Container
                .GetItemLinqQueryable<T>(requestOptions: CosmosRepositoryExtensions.GetQueryRequestOptions())
                .Where(predicate?.Compose(item => item.Type == type, Expression.AndAlso) ?? (item => item.Type == type));

            if (transform != null) queryable = transform(queryable);

            using var iterator = queryable.ToFeedIterator();
            var results = new List<T>();

            double charges = 0;
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                charges += response.RequestCharge;
                results.AddRange(response.Resource);
            }

            if (charges > 10d + extra)
                LogMessages.RequestCharge(Logger, "Query", type.ToString(), charges);

            return results;
        }
        catch (CosmosOperationCanceledException)
        {
            return [];
        }
    }

    public async Task BulkUpsertAsync<T>(IEnumerable<T> items) where T : GroupDocument
    {
        if (items.Empty()) return;

        var groupedItems = items.GroupBy(item => (int)item.Type);

        foreach (var group in groupedItems)
        {
            var partitionKey = new PartitionKey(group.Key);
            const int batchSize = 100;
            var batches = group
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.item));

            foreach (var batch in batches)
            {
                var transactionalBatch = Container.CreateTransactionalBatch(partitionKey);
                double charges = 0;

                foreach (var item in batch)
                {
                    transactionalBatch.UpsertItem(item);
                }

                var batchResponse = await transactionalBatch.ExecuteAsync();
                charges += batchResponse.RequestCharge;

                if (!batchResponse.IsSuccessStatusCode)
                {
                    throw new UnhandledException($"Batch update failed with status code: {batchResponse.StatusCode}");
                }

                if (charges > 1100)
                    Logger.RequestCharge("BulkUpsertAsync", group.Key.ToString(CultureInfo.InvariantCulture), charges);
            }
        }
    }
}