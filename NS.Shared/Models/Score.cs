namespace NS.Shared.Models
{
    public class Score() : ProtectedMainDocument(DocumentType.Score)
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public List<ScoreDetail> Items { get; set; } = [];
    }

    public class ScoreDetail
    {
        public string? Code { get; set; }
        public double? Value { get; set; }

        public double? CalculatePassportIndex()
        {
            int existingPassports = 200;

            if (Value.HasValue)
            {
                if (Value.Value >= existingPassports)
                {
                    return 10.0;
                }
                else
                {
                    var result = Value.Value / existingPassports * 10;
                    return Math.Round(result, 2);
                }
            }
            return null;
        }
    }
}