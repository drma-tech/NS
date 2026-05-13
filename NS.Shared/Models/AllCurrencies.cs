namespace NS.Shared.Models;

public class AllCurrencies
{
    public List<CurrencyModel> Items { get; set; } = [];

    public CurrencyModel? GetByCode(string? code)
    {
        return Items.SingleOrDefault(f => f.code!.Equals(code, StringComparison.OrdinalIgnoreCase));
    }
}

public class CurrencyModel
{
    public string? code { get; set; }
    public string? name { get; set; }
}