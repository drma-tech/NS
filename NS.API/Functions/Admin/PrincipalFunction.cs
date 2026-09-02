using Clerk.BackendAPI;
using Clerk.BackendAPI.Models.Operations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.Shared.Core.Types;
using NS.Shared.Models.Auth;

namespace NS.API.Functions.Admin;

public class PrincipalFunction(CosmosMainRepository repo)
{
    //private const string CloneFailed = "DeepClone failed";

    //[Function("PrincipalMigrate")]
    //public async Task PrincipalMigrate(
    //    [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "principal/migrate")] HttpRequestData req, CancellationToken cancellationToken)
    //{
    //    var principais = await repo.Query<AuthPrincipal>(MainType.Principal, p => !p.UserId!.StartsWith("user_"), transform: null, cancellationToken);
    //    var sdk = new ClerkBackendApi(bearerAuth: ApiStartup.Configurations.ClerkAuth!.SecretKey);

    //    foreach (var principal in principais)
    //    {
    //        try
    //        {
    //            var request = new CreateUserRequestBody()
    //            {
    //                FirstName = principal.DisplayName?.Split(" ").ElementAtIndex(0),
    //                LastName = principal.DisplayName?.Split(" ").ElementAtIndex(1),
    //                EmailAddress = [principal.Email!],
    //            };

    //            var user = await sdk.Users.CreateAsync(request);

    //            var clone = principal.DeepClone() ?? throw new NotificationException(CloneFailed);
    //            clone.ChangeIdentity(new MainIdentity(MainType.Principal, user.User!.Id));
    //            clone.UserId = user.User.Id;
    //            await repo.CreateItemAsync(clone);
    //            await repo.DeleteItemAsync<AuthPrincipal>(new MainIdentity(MainType.Principal, principal.Id));

    //            var myLogins = await repo.ReadItemAsync<AuthLogin>(new MainIdentity(MainType.Login, principal.Id), cancellationToken);
    //            if (myLogins != null)
    //            {
    //                var model = myLogins.DeepClone() ?? throw new NotificationException(CloneFailed);
    //                model.ChangeIdentity(new MainIdentity(MainType.Login, user.User.Id));
    //                model.UserId = user.User.Id;
    //                await repo.CreateItemAsync(model);
    //                await repo.DeleteItemAsync<AuthLogin>(new MainIdentity(MainType.Login, principal.Id));
    //            }

    //            var myDest = await repo.ReadItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, principal.Id), cancellationToken);
    //            if (myDest != null)
    //            {
    //                var model = myDest.DeepClone() ?? throw new NotificationException(CloneFailed);
    //                model.ChangeIdentity(new MainIdentity(MainType.NextDestinations, user.User.Id));
    //                await repo.CreateItemAsync(model);
    //                await repo.DeleteItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, principal.Id));
    //            }

    //            var myHist = await repo.ReadItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, principal.Id), cancellationToken);
    //            if (myHist != null)
    //            {
    //                var model = myHist.DeepClone() ?? throw new NotificationException(CloneFailed);
    //                model.ChangeIdentity(new MainIdentity(MainType.TravelHistory, user.User.Id));
    //                await repo.CreateItemAsync(model);
    //                await repo.DeleteItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, principal.Id));
    //            }

    //            var myWish = await repo.ReadItemAsync<WishList>(new MainIdentity(MainType.WishList, principal.Id), cancellationToken);
    //            if (myWish != null)
    //            {
    //                var model = myWish.DeepClone() ?? throw new NotificationException(CloneFailed);
    //                model.ChangeIdentity(new MainIdentity(MainType.WishList, user.User.Id));
    //                await repo.CreateItemAsync(model);
    //                await repo.DeleteItemAsync<WishList>(new MainIdentity(MainType.WishList, principal.Id));
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //}
}
