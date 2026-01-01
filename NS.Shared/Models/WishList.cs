namespace NS.Shared.Models;

public class WishList() : PrivateMainDocument(DocumentType.WishList)
{
    public HashSet<string> Regions { get; init; } = [];
}