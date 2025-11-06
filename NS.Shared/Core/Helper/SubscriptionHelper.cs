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
}

public abstract class Restrictions
{
    public abstract int Energy { get; }
}

public class BasicRestrictions : Restrictions
{
    public override int Energy => 10;
}

public class StandardRestrictions : Restrictions
{
    public override int Energy => 50;
}

public class PremiumRestrictions : Restrictions
{
    public override int Energy => 100;
}