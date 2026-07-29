using NS.Shared.Core.Types;

namespace NS.Shared.Core.Models;

public readonly record struct GroupIdentity(GroupType Type, string? DocId) : ICosmosIdentity
{
    public string Id => $"{Type}:{DocId.RemovePrefix()}";
    public string? RawId => DocId?.RemovePrefix();
    public object Key => (int)Type;
}

public abstract class GroupDocument(GroupIdentity identity) : CosmosDocument(identity)
{
    public GroupType Type { get; set; } = identity.Type;
}
