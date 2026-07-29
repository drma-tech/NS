using NS.Shared.Core.Types;

namespace NS.Shared.Models
{
    public class Suggestion(string? id) : GroupDocument(new GroupIdentity(GroupType.Suggestion, id))
    {
        public string? Icon { get; set; }
        public List<SuggestionRegion> Regions { get; set; } = [];
    }

    public class SuggestionRegion
    {
        public int Index { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoId { get; set; }
        public string? CustomPhotoUrl { get; set; }
        public string? CustomPhotoCredit { get; set; }
        public RegionModel? Region { get; set; }
    }
}