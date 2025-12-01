using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using NS.API.Repository.Core;
using System.Linq.Expressions;
using System.Net;

namespace NS.API.Repository;

public class CosmosGroupRepository
{
    private readonly ILogger<CosmosGroupRepository> _logger;

    public CosmosGroupRepository(CosmosClient CosmosClient, ILogger<CosmosGroupRepository> logger)
    {
        _logger = logger;

        var databaseId = ApiStartup.Configurations.CosmosDB?.DatabaseId;

        Container = CosmosClient.GetContainer(databaseId, "group");
    }

    public Container Container { get; }

    public async Task<T?> Get<T>(DocumentType type, string? id, CancellationToken cancellationToken)
        where T : MainDocument
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        try
        {
            var response = await Container.ReadItemAsync<T>($"{type}:{id}", new PartitionKey((int)type),
                CosmosRepositoryExtensions.GetItemRequestOptions(), cancellationToken);

            if (response.RequestCharge > 2)
                _logger.LogWarning("Get - ID {Id}, RequestCharge {Charges}", id, response.RequestCharge);

            return response.Resource;
        }
        catch (CosmosOperationCanceledException)
        {
            return null;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<List<T>> ListAll<T>(DocumentType type, CancellationToken cancellationToken)
        where T : MainDocument
    {
        try
        {
            var query = Container.GetItemLinqQueryable<T>(requestOptions: CosmosRepositoryExtensions.GetQueryRequestOptions(new PartitionKey((int)type)));

            using var iterator = query.ToFeedIterator();
            var results = new List<T>();

            double charges = 0;
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                charges += response.RequestCharge;
                results.AddRange(response.Resource);
            }

            if (charges > 12)
                _logger.LogWarning("ListAll - Type {Type}, RequestCharge {Charges}", type.ToString(), charges);

            return results;
        }
        catch (CosmosOperationCanceledException)
        {
            return [];
        }
    }

    public async Task<List<T>> Query<T>(Expression<Func<T, bool>> predicate, DocumentType type, CancellationToken cancellationToken)
        where T : MainDocument
    {
        try
        {
            var query = Container
                .GetItemLinqQueryable<T>(requestOptions: CosmosRepositoryExtensions.GetQueryRequestOptions(new PartitionKey((int)type)))
                .Where(predicate.Compose(item => item.Type == type, Expression.AndAlso));

            using var iterator = query.ToFeedIterator();
            var results = new List<T>();

            double charges = 0;
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                charges += response.RequestCharge;
                results.AddRange(response.Resource);
            }

            if (charges > 30)
                _logger.LogWarning("Query - Type {Type}, RequestCharge {Charges}", type.ToString(), charges);

            return results;
        }
        catch (CosmosOperationCanceledException)
        {
            return [];
        }
    }

    public async Task<T> CreateItemAsync<T>(T item, CancellationToken cancellationToken)
        where T : MainDocument, new()
    {
        try
        {
            var response = await Container.CreateItemAsync(item, new PartitionKey((int)item.Type), CosmosRepositoryExtensions.GetItemRequestOptions(), cancellationToken);

            if (response.RequestCharge > 20)
                _logger.LogWarning("CreateItemAsync - ID {Id}, RequestCharge {Charges}", item.Id, response.RequestCharge);

            return response.Resource;
        }
        catch (CosmosOperationCanceledException)
        {
            return new T();
        }
    }

    public async Task<T> UpsertItemAsync<T>(T item, CancellationToken cancellationToken)
        where T : MainDocument, new()
    {
        try
        {
            var response = await Container.UpsertItemAsync(item, new PartitionKey((int)item.Type), CosmosRepositoryExtensions.GetItemRequestOptions(), cancellationToken);

            if (response.RequestCharge > 20)
                _logger.LogWarning("UpsertItemAsync - ID {Id}, RequestCharge {Charges}", item.Id, response.RequestCharge);

            return response.Resource;
        }
        catch (CosmosOperationCanceledException)
        {
            return new T();
        }
    }

    public async Task BulkUpsertAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken)
        where T : MainDocument
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

                var batchResponse = await transactionalBatch.ExecuteAsync(cancellationToken);
                charges += batchResponse.RequestCharge;

                if (!batchResponse.IsSuccessStatusCode)
                {
                    throw new UnhandledException($"Batch update failed with status code: {batchResponse.StatusCode}");
                }

                if (charges > 1100)
                    _logger.LogWarning("BulkUpsertAsync - Type {Type}, RequestCharge {Charges}", group.Key, charges);
            }
        }
    }

    /// <summary>
    ///     to update arrays, there is no performance gain
    /// </summary>
    public async Task<T> PatchItem<T>(DocumentType type, string? id, List<PatchOperation> operations, CancellationToken cancellationToken)
        where T : MainDocument, new()
    {
        //https://learn.microsoft.com/en-us/azure/cosmos-db/partial-document-update-getting-started?tabs=dotnet

        try
        {
            var response = await Container.PatchItemAsync<T>($"{type}:{id}", new PartitionKey((int)type),
                operations, CosmosRepositoryExtensions.GetPatchItemRequestOptions(), cancellationToken);

            if (response.RequestCharge > 20)
                _logger.LogWarning("PatchItem - ID {Id}, RequestCharge {Charges}", id, response.RequestCharge);

            return response.Resource;
        }
        catch (CosmosOperationCanceledException)
        {
            return new T();
        }
    }

    public async Task<bool> Delete<T>(T item, CancellationToken cancellationToken)
        where T : MainDocument
    {
        try
        {
            var response = await Container.DeleteItemAsync<T>(item.Id, new PartitionKey((int)item.Type),
                CosmosRepositoryExtensions.GetItemRequestOptions(), cancellationToken);

            if (response.RequestCharge > 20)
                _logger.LogWarning("Delete - ID {Id}, RequestCharge {Charges}", item.Id, response.RequestCharge);

            return response.StatusCode == HttpStatusCode.OK;
        }
        catch (CosmosOperationCanceledException)
        {
            return false;
        }
    }

    //Overview of indexing in Azure Cosmos DB
    //https://learn.microsoft.com/en-us/azure/cosmos-db/index-overview
}