using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace NS.Shared.Core.Helper
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CustomAppAttribute : Attribute
    {
        public required string Icon { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }

        public Type? ResourceType { get; set; }
    }

    public static class CustomAppAttributeHelper
    {
        public static CustomAppAttribute? GetCustomAppAttribute(this Enum value, bool translate = true)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            return fieldInfo?.GetCustomAppAttribute(translate);
        }

        public static CustomAppAttribute GetCustomAppAttribute(this MemberInfo mi, bool translate = true)
        {
            if (mi.GetCustomAttribute<CustomAppAttribute>() is not CustomAppAttribute attr)
                throw new ValidationException($"Attribute '{mi.Name}' is null");

            if (translate && attr.ResourceType != null) //translations
            {
                var rm = new ResourceManager(attr.ResourceType.FullName ?? "", attr.ResourceType.Assembly);

                if (!string.IsNullOrEmpty(attr.Icon))
                    attr.Icon = rm.GetString(attr.Icon) ?? attr.Icon + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Name))
                    attr.Name = rm.GetString(attr.Name) ?? attr.Name + " (incomplete translation)";
                if (!string.IsNullOrEmpty(attr.Url))
                    attr.Url = rm.GetString(attr.Url) ?? attr.Url + " (incomplete translation)";
            }

            return attr;
        }
    }
}