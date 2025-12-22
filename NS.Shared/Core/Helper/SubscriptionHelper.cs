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
}

public abstract class Restrictions
{
    public abstract int Energy { get; }
}

public class BasicRestrictions : Restrictions
{
    public override int Energy => 5;
}

public class PremiumRestrictions : Restrictions
{
    public override int Energy => 100;
}
