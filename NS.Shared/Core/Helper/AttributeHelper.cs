using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace NS.Shared.Core.Helper
{
    public sealed class EnumFieldObject<T>(string name, T value) where T : Enum
    {
        public T Value { get; set; } = value;
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
    }

    public static class AttributeHelper
    {
        private static readonly ConcurrentDictionary<MemberInfo, FieldSettingsAttribute> AttributeCache = new();
        private static readonly ConcurrentDictionary<Type, ResourceManager> ResourceManagers = new();

        private const string IncompleteTranslationSuffix = " (incomplete translation)";

        public static EnumFieldObject<T> GetFieldSettings<T>(this T value, bool translate = true) where T : Enum
        {
            var fieldInfo = value.GetType().GetField(value.ToString()) ?? throw new UnhandledException($"{value} field info is null");

            return fieldInfo.GetFieldSettings(value, translate);
        }

        private static EnumFieldObject<T> GetFieldSettings<T>(this MemberInfo mi, T value, bool translate = true) where T : Enum
        {
            var attr = AttributeCache.GetOrAdd(mi, x => x.GetCustomAttribute<FieldSettingsAttribute>() ?? throw new ValidationException($"Field Settings '{x.Name}' is null"));

            var obj = new EnumFieldObject<T>(attr.Name, value)
            {
                Group = attr.Group,
                Placeholder = attr.Placeholder,
                Description = attr.Description,
                Icon = attr.Icon,
                Url = attr.Url,
                Proportion = attr.Proportion,
                ShortTitle = attr.ShortTitle,
                Title = attr.Title,
                SubTitle = attr.SubTitle
            };

            ApplyTranslations(obj, attr, translate);

            return obj;
        }

        private static void ApplyTranslations<T>(EnumFieldObject<T> obj, FieldSettingsAttribute attr, bool translate) where T : Enum
        {
            if (attr.ResourceType != null && translate)
            {
                var rm = ResourceManagers.GetOrAdd(attr.ResourceType, t => new ResourceManager(t.FullName!, t.Assembly));

                obj.Name = rm.GetResourceString(attr.Name) ?? throw new InvalidOperationException($"Resource not found for key: {attr.Name}");
                if (attr.Group.NotEmpty()) obj.Group = rm.GetResourceString(attr.Group);
                if (attr.Placeholder.NotEmpty()) obj.Placeholder = rm.GetResourceString(attr.Placeholder)?.Replace(@"\n", Environment.NewLine);
                if (attr.Description.NotEmpty()) obj.Description = rm.GetResourceString(attr.Description);
                if (attr.Icon.NotEmpty()) obj.Icon = rm.GetResourceString(attr.Icon);
                if (attr.Url.NotEmpty()) obj.Url = rm.GetResourceString(attr.Url);
                if (attr.ShortTitle.NotEmpty()) obj.ShortTitle = rm.GetResourceString(attr.ShortTitle);
                if (attr.Title.NotEmpty()) obj.Title = rm.GetResourceString(attr.Title);
                if (attr.SubTitle.NotEmpty()) obj.SubTitle = rm.GetResourceString(attr.SubTitle);
            }
        }

        private static string GetResourceString(this ResourceManager rm, string resourceKey)
        {
            return rm.GetString(resourceKey) ?? resourceKey + IncompleteTranslationSuffix;
        }
    }
}
