namespace NS.Shared.Models;

public class WishList() : PrivateMainDocument(DocumentType.WishList)
{
    public HashSet<Enums.Region> Regions { get; init; } = [];
}