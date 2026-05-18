namespace NS.Shared.Models.GlobalConflicts
{
    public class GlobalConflicts
    {
        public List<Item> Items { get; set; } = [];
    }

    public class Item
    {
        public Item()
        {
        }

        public Item(string? title, string? type, string? status, List<string> regions)
        {
            this.title = title;
            this.type = type;
            this.status = status;
            this.regions = regions;
        }

        public string? title { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public List<string> regions { get; set; } = [];
    }
}