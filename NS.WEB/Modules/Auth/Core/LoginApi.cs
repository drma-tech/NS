using NS.Shared.Models.Auth;

namespace NS.WEB.Modules.Auth.Core;

public class LoginApi(IHttpClientFactory factory) : ApiCosmos<AuthLogin>(factory, ApiType.Authenticated, null)
{
    public async Task<AuthLogin?> Get(bool isUserAuthenticated)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get);

        return null;
    }

    public async Task Add(NS.Shared.Enums.Platform platform)
    {
        await PostAsync<AuthLogin>(Endpoint.Add(platform.ToString()), null);
    }

    private struct Endpoint
    {
        public const string Get = "login/get";

        public static string Add(string platform)
        {
            return $"login/add?platform={platform}";
        }
    }
}
