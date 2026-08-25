using NS.Shared.Models.Auth;
using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Authenticated
{
    public class PaymentAuthApi(IHttpClientFactory factory) : ApiCosmos<AuthSubscription>(factory, ApiType.Authenticated, key: null, [], ApiContext.Default.AuthSubscription)
    {
        public async Task AppleVerify(string receipt, CancellationToken cancellationToken)
        {
            await PostAsync("apple/verify", receipt, ApiContext.Default.String, states: [], cancellationToken);
        }

        public async Task<string?> StripePortalLink(CancellationToken cancellationToken)
        {
            return await GetStringAsync("stripe/portal-link", cancellationToken);
        }
    }
}