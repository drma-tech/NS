namespace NS.Shared.Core.Helper;

[AttributeUsage(AttributeTargets.Field)]
public class FieldSettingsAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
    public string? Group { get; set; }
    public string? Placeholder { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public double Proportion { get; set; } = 1;
    public string? ShortTitle { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }

    public Type? ResourceType { get; set; }
}
