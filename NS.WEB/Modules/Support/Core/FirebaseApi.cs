using NS.Shared.Models.Auth;

namespace NS.WEB.Modules.Support.Core
{
    public class FirebaseApi(IHttpClientFactory factory) : ApiCosmos<AuthLogin>(factory, ApiType.Anonymous, null)
    {
        public async Task Subscribe(string token, string platform)
        {
            ArgumentNullException.ThrowIfNull(token);
            ArgumentNullException.ThrowIfNull(platform);

            await PostAsync<AuthPrincipal>(Endpoint.Subscribe(token, platform), null);
        }

        private struct Endpoint
        {
            public static string Subscribe(string token, string platform) => $"firebase/subscribe?token={token}&platform={platform}";
        }
    }
}
