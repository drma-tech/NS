namespace NS.Shared.Enums
{
    public enum PaymentMethods
    {
        //Cards (10)

        [Custom(Name = "Mastercard")]
        Mastercard = 10,

        [Custom(Name = "Visa")]
        Visa = 11,

        [Custom(Name = "Diners Club")]
        DinersClub = 12,

        [Custom(Name = "Discover")]
        Discover = 13,

        [Custom(Name = "UnionPay")]
        UnionPay = 14,

        [Custom(Name = "American Express")]
        AmericanExpress = 15,

        [Custom(Name = "JCB")]
        JCB = 16,

        [Custom(Name = "Maestro")]
        Maestro = 17,

        //Mobile (20)

        [Custom(Name = "Apple Pay")]
        ApplePay = 20,

        [Custom(Name = "Google Pay")]
        GooglePay = 21,

        //e-Wallet (300)

        [Custom(Name = "WeChat Pay")]
        WeChatPay = 30,

        [Custom(Name = "Alipay+")]
        Alipay = 31,

        [Custom(Name = "Skrill")]
        Skrill = 32,

        [Custom(Name = "PayPal")]
        PayPal = 33,
    }
}