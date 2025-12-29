using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace NS.Shared.Core.Helper
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CustomCountryAttribute : Attribute
    {
        public required string Continent { get; set; }
        public string? Subcontinent { get; set; }
        public required string Name { get; set; }
        public required string FullName { get; set; }
        public required string Capital { get; set; }
        public string? Description { get; set; }

        public Type? ResourceType { get; set; }
    }

    public static class CustomCountryAttributeHelper
    {
        public static CustomCountryAttribute? GetCustomCountryAttribute(this Enum value, bool translate = true)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            return fieldInfo?.GetCustomCountryAttribute(translate);
        }

        public static CustomCountryAttribute GetCustomCountryAttribute(this MemberInfo mi, bool translate = true)
        {
            if (mi.GetCustomAttribute<CustomCountryAttribute>() is not CustomCountryAttribute attr)
                throw new ValidationException($"Attribute '{mi.Name}' is null");

            if (translate && attr.ResourceType != null) //translations
            {
                var rm = new ResourceManager(attr.ResourceType.FullName ?? "", attr.ResourceType.Assembly);

                if (!string.IsNullOrEmpty(attr.Continent))
                    attr.Continent = rm.GetString(attr.Continent) ?? attr.Continent + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Subcontinent))
                    attr.Subcontinent = rm.GetString(attr.Subcontinent) ?? attr.Subcontinent + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Name))
                    attr.Name = rm.GetString(attr.Name) ?? attr.Name + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.FullName))
                    attr.FullName = rm.GetString(attr.FullName) ?? attr.FullName + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Description))
                    attr.Description = rm.GetString(attr.Description) ?? attr.Description + " (incomplete translation)";
            }

            return attr;
        }
    }
}