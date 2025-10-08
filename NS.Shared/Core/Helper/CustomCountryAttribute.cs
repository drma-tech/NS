using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace NS.Shared.Core.Helper
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CustomCountryAttribute : Attribute
    {
        public required string Region { get; set; }
        public string? Subregion { get; set; }
        public required string Name { get; set; }
        public required string FullName { get; set; }
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

                if (!string.IsNullOrEmpty(attr.Region))
                    attr.Region = rm.GetString(attr.Region) ?? attr.Region + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Subregion))
                    attr.Subregion = rm.GetString(attr.Subregion) ?? attr.Subregion + " (incomplete translation)";
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