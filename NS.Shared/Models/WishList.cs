namespace NS.Shared.Models;

public class WishList() : PrivateMainDocument(DocumentType.WishList)
{
    public HashSet<Enums.Country> Countries { get; init; } = [];
}