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
            throw new NotificationException("Your current plan does not support this operation");
    }
}

public abstract class Restrictions
{
    public abstract int Energy { get; }
    public abstract int Wishlist { get; }
}

public class BasicRestrictions : Restrictions
{
    public override int Energy => 5;
    public override int Wishlist => 3;
}

public class PremiumRestrictions : Restrictions
{
    public override int Energy => 100;
    public override int Wishlist => 50;
}