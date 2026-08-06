using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NS.WEB.Shared
{
    public partial class SettingsPopup
    {
        [CascadingParameter] private IMudDialogInstance? MudDialog { get; set; }

        private readonly IEnumerable<EnumFieldObject<AppLanguage>> appLanguages = EnumHelper.GetList<AppLanguage>();

        public AppLanguage Language { get; set; } = AppLanguage.en;
        public bool DarkMode { get; set; }
        public Temperature Temperature { get; set; } = Temperature.Celsius;

        protected override async Task<bool> LoadInteropDataAsync(Microsoft.JSInterop.IJSRuntime JsRuntime)
        {
            Language = await AppStateStatic.GetAppLanguage(JsRuntime, Cts.Token);
            DarkMode = await AppStateStatic.GetDarkMode(JsRuntime, Cts.Token) ?? false;
            Temperature = await AppStateStatic.GetTemperature(JsRuntime, Cts.Token) ?? Temperature.Celsius;

            return true;
        }

        protected async Task AppLanguageValueChanged(AppLanguage value)
        {
            Language = value;

            await JsRuntime.Utils().SetStorage("app-language", value, JavascriptContext.Default.AppLanguage, Cts.Token);

            var uri = new Uri(Navigation.Uri);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(1);
            var newPath = $"/{value}/{string.Join('/', segments)}";

            Navigation.NavigateTo($"{newPath}{uri.Query}".TrimEnd('/'), forceLoad: true);
        }

        protected async Task DarkModeChanged(bool value)
        {
            DarkMode = value;

            await JsRuntime.Utils().SetStorage("dark-mode", value, JavascriptContext.Default.Boolean, Cts.Token);

            AppStateStatic.ChangeDarkMode(value);
        }

        protected async Task TemperatureChanged(Temperature value)
        {
            Temperature = value;

            await JsRuntime.Utils().SetStorage("temperature", value, JavascriptContext.Default.Temperature, Cts.Token);

            AppStateStatic.ChangeTemperature(value);
        }
    }
}
