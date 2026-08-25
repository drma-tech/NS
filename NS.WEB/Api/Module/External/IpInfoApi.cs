using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.External
{
    public class IpInfoApi(IHttpClientFactory factory) : ApiExternal(factory)
    {
        public async Task<string?> GetCountry(CancellationToken cancellationToken)
        {
            try
            {
                return await GetStringAsync("public/country", cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}