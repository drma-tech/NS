using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using NS.API.Repository.Core;

namespace NS.API.Repository;

public class CosmosCacheRepository(CosmosClient client, ILogger<CosmosCacheRepository> logger)
     : BaseRepository<CosmosCacheRepository, CacheDocument, CacheIdentity>(client, logger, "cache")
{
}
