namespace NS.Shared.Core.Helper;

public static class SubscriptionHelper
{
    public static Restrictions GetRestrictions(this AccountProduct product)
    {
        return product switch
        {
            AccountProduct.Basic => new BasicRestrictions(),
            AccountProduct.Standard => new StandardRestrictions(),
            AccountProduct.Premium => new PremiumRestrictions(),
            _ => new BasicRestrictions()
        };
    }

    public static void ValidateFavoriteProviders(AccountProduct? product, int qtd)
    {
        product ??= AccountProduct.Basic;
        var restriction = product.Value.GetRestrictions();

        if (qtd > restriction.FavoriteProviders)
            throw new NotificationException("Your current plan does not support this operation");
    }
}

public abstract class Restrictions
{
    public abstract int FavoriteProviders { get; }
}

public class BasicRestrictions : Restrictions
{
    public override int FavoriteProviders => 2;
}

public class StandardRestrictions : Restrictions
{
    public override int FavoriteProviders => 10;
}

public class PremiumRestrictions : Restrictions
{
    public override int FavoriteProviders => 20;
}