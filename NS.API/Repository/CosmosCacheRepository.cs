using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using NS.API.Repository.Core;

namespace NS.API.Repository;

public class CosmosCacheRepository(CosmosClient CosmosClient, ILogger<CosmosCacheRepository> logger)
     : BaseRepository<CosmosCacheRepository, CacheDocument, CacheIdentity>(CosmosClient, logger, "cache")
{
}
