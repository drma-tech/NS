namespace NS.API.Core.Models
{
    public class ConflictData
    {
        public ConflictDetail[] rows { get; set; } = [];
    }

    public class ConflictDetail
    {
        public string? country { get; set; }
        public string? level { get; set; }
    }
}