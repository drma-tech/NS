namespace NS.Shared.Core.Helper;

public static class SubscriptionHelper
{
    public static Restrictions GetRestrictions(this AccountProduct product)
    {
        return product switch
        {
            AccountProduct.Basic => new BasicRestrictions(),
            AccountProduct.Premium => new PremiumRestrictions(),
            _ => new BasicRestrictions()
        };
    }

    public static void ValidateWishList(AccountProduct? product, int qtd)
    {
        product ??= AccountProduct.Basic;
        var restriction = product.Value.GetRestrictions();

        if (qtd > restriction.Wishlist)
            throw new NotificationException("Your current plan does not support this operation. Consider upgrading to premium for more benefits.");
    }

    public static void ValidateTravelHistory(AccountProduct? product, int qtd)
    {
        product ??= AccountProduct.Basic;
        var restriction = product.Value.GetRestrictions();

        if (qtd > restriction.TravelHistory)
            throw new NotificationException("Your current plan does not support this operation. Consider upgrading to premium for more benefits.");
    }

    public static void ValidateNextDestinations(AccountProduct? product, int qtd)
    {
        product ??= AccountProduct.Basic;
        var restriction = product.Value.GetRestrictions();

        if (qtd > restriction.NextDestinations)
            throw new NotificationException("Your current plan does not support this operation. Consider upgrading to premium for more benefits.");
    }
}

public abstract class Restrictions
{
    public abstract int Energy { get; }
    public abstract int Wishlist { get; }
    public abstract int TravelHistory { get; }
    public abstract int NextDestinations { get; }
}

public class BasicRestrictions : Restrictions
{
    public override int Energy => 10;
    public override int Wishlist => 3;
    public override int TravelHistory => 5;
    public override int NextDestinations => 1;
}

public class PremiumRestrictions : Restrictions
{
    public override int Energy => 100;
    public override int Wishlist => 50;
    public override int TravelHistory => 250;
    public override int NextDestinations => 10;
}