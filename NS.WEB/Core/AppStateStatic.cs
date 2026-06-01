using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using NS.Shared.Models.Auth;
using NS.WEB.Modules.Subscription.Core;
using System.Security.Claims;

namespace NS.WEB.Core;

public static class AppStateStatic
{
    public static string? SupabaseToken { get; set; }
    public static bool IsAuthenticated { get; set; }
    public static bool IsPremiumUser { get; set; }
    public static AccountProduct ActiveProduct { get; set; }
    public static ClaimsPrincipal? User { get; set; }
    public static AuthPrincipal? Principal { get; set; }
    public static string? UserId { get; set; }
    public static DateTimeOffset? LastAccess { get; set; } //control login, so we don't call api too often

    public static Size Size { get; set; } = Size.Small;
    public static Breakpoint Breakpoint { get; set; } = Breakpoint.Xs;
    public static ActionDispatcher<Breakpoint> BreakpointChanged { get; } = new();

    public static string? Version { get; set; }
    public static string? BrowserName { get; set; }
    public static string? BrowserVersion { get; set; }
    public static string? OperatingSystem { get; set; }

    private static string? LastSnackbarMessage { get; set; }
    private static DateTime LastSnackbarAt { get; set; } = DateTime.MinValue;
    private static readonly TimeSpan SnackbarDelay = TimeSpan.FromSeconds(5);

    public static bool IsLocalhost(this NavigationManager navigation)
    {
        return navigation.BaseUri.Contains("localhost") || navigation.BaseUri.Contains("develop");
    }

    public static bool IsPrerendering(this NavigationManager navigation)
    {
        return navigation.BaseUri.Contains("127.0.0.1");
    }

    public static bool CanShowSnackbar(this string message)
    {
        var now = DateTime.UtcNow;

        if (LastSnackbarMessage == message &&
            now - LastSnackbarAt < SnackbarDelay)
        {
            return false;
        }

        LastSnackbarMessage = message;
        LastSnackbarAt = now;

        return true;
    }

    public static async Task<string> GetAppVersion(IJSRuntime js, CancellationToken cancellationToken)
    {
        try
        {
            var vs = await js.Utils().GetAppVersion(cancellationToken);

            return vs?.ReplaceLineEndings("").Trim() ?? "version-error";
        }
        catch (Exception)
        {
            return "version-error";
        }
    }

    #region Platform

    private static Platform? _platform;
    private static readonly SemaphoreSlim _platformSemaphore = new(1, 1);

    public static Platform? GetSavedPlatform()
    {
        return _platform;
    }

    public static async Task<Platform> GetPlatform(IJSRuntime js, CancellationToken cancellationToken)
    {
        await _platformSemaphore.WaitAsync(cancellationToken);
        try
        {
            if (_platform.HasValue)
            {
                return _platform.Value;
            }

            var cache = await js.Utils().GetStorage("platform", JavascriptContext.Default.NullablePlatform, cancellationToken);

            if (cache.HasValue)
            {
                _platform = cache.Value;
            }
            else
            {
                _platform = Platform.webapp;
                await js.Utils().SetStorage("platform", _platform, JavascriptContext.Default.NullablePlatform, cancellationToken);
            }

            return _platform.Value;
        }
        finally
        {
            _platformSemaphore.Release();
        }
    }

    #endregion Platform

    #region AppLanguage

    public static string[] SupportedLanguages => ["en", "pt", "es", "zh", "fr", "it", "de"];

    private static AppLanguage? _appLanguage;
    private static readonly SemaphoreSlim _appLanguageSemaphore = new(1, 1);

    public static bool IsValidLanguage(this string? lang)
    {
        return SupportedLanguages.Contains(lang);
    }

    public static async Task<AppLanguage> GetAppLanguage(IJSRuntime js, CancellationToken cancellationToken)
    {
        await _appLanguageSemaphore.WaitAsync(cancellationToken);
        try
        {
            if (_appLanguage.HasValue)
            {
                return _appLanguage.Value;
            }

            var cache = await js.Utils().GetStorage("app-language", JavascriptContext.Default.NullableAppLanguage, cancellationToken);

            if (cache.HasValue)
            {
                _appLanguage = cache.Value;
            }
            else
            {
                var code = await js.Window().InvokeAsync<string>("eval", "navigator.language");
                code = code[..2].ToLowerInvariant();

                _appLanguage = ConvertAppLanguage(code, AppLanguage.en);
                await js.Utils().SetStorage("app-language", _appLanguage, JavascriptContext.Default.NullableAppLanguage, cancellationToken);
            }

            return _appLanguage.Value;
        }
        catch
        {
            return AppLanguage.en;
        }
        finally
        {
            _appLanguageSemaphore.Release();
        }
    }

    public static AppLanguage ConvertAppLanguage(string? code, AppLanguage fallback)
    {
        if (code.Empty()) return AppLanguage.en;

        if (System.Enum.TryParse<AppLanguage>(code, true, out var language) && System.Enum.IsDefined(language))
        {
            return language;
        }
        else
            return fallback;
    }

    public static string GetCulture(this NavigationManager navigation)
    {
        var segments = new Uri(navigation.Uri).AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var first = segments.FirstOrDefault()?.ToLowerInvariant();

        return first.IsValidLanguage() ? first! : "en";
    }

    #endregion AppLanguage

    #region DarkMode

    public static Action<bool>? DarkModeChanged { get; set; }

    private static bool? _darkMode;
    private static readonly SemaphoreSlim _darkModeSemaphore = new(1, 1);

    public static async Task<bool?> GetDarkMode(IJSRuntime js, CancellationToken cancellationToken)
    {
        await _darkModeSemaphore.WaitAsync(cancellationToken);
        try
        {
            if (_darkMode.HasValue)
            {
                return _darkMode.Value;
            }

            _darkMode = await js.Utils().GetStorage("dark-mode", JavascriptContext.Default.NullableBoolean, cancellationToken);

            return _darkMode;
        }
        catch
        {
            return null;
        }
        finally
        {
            _darkModeSemaphore.Release();
        }
    }

    public static void ChangeDarkMode(bool darkMode)
    {
        _darkMode = darkMode;
        DarkModeChanged?.Invoke(darkMode);
    }

    #endregion DarkMode

    #region Region Country

    private static string? _country;
    private static readonly SemaphoreSlim _countrySemaphore = new(1, 1);

    public static string? GetSavedCountry()
    {
        return _country;
    }

    public static async Task<string?> GetCountry(IpInfoApi api, IJSRuntime js, CancellationToken cancellationToken)
    {
        await _countrySemaphore.WaitAsync(cancellationToken);
        try
        {
            if (_country.NotEmpty())
            {
                return _country;
            }

            var cache = await js.Utils().GetStorage("country", JavascriptContext.Default.String, cancellationToken);

            if (cache.NotEmpty())
            {
                _country = cache.Trim();
            }
            else
            {
                _country = (await api.GetCountry(cancellationToken))?.Trim();

                if (_country.NotEmpty())
                    await js.Utils().SetStorage("country", _country, JavascriptContext.Default.String, cancellationToken);
            }

            return _country;
        }
        finally
        {
            _countrySemaphore.Release();
        }
    }

    #endregion Region Country

    #region Temperature

    public static Action<Temperature>? TemperatureChanged { get; set; }

    private static Temperature? _temperature;
    private static readonly SemaphoreSlim _temperatureSemaphore = new(1, 1);

    public static async Task<Temperature?> GetTemperature(IJSRuntime js, CancellationToken cancellationToken)
    {
        await _temperatureSemaphore.WaitAsync();
        try
        {
            if (_temperature.HasValue)
            {
                return _temperature.Value;
            }

            var cache = await js.Utils().GetStorage<Temperature?>("temperature", JavascriptContext.Default.NullableTemperature, cancellationToken);

            if (cache.HasValue)
            {
                _temperature = cache.Value;
            }
            else
            {
                _temperature = Temperature.Celsius;
                await js.Utils().SetStorage("temperature", _temperature, JavascriptContext.Default.NullableTemperature, cancellationToken);
            }

            return _temperature;
        }
        catch
        {
            return null;
        }
        finally
        {
            _temperatureSemaphore.Release();
        }
    }

    public static void ChangeTemperature(Temperature value)
    {
        _temperature = value;
        TemperatureChanged?.Invoke(value);
    }

    #endregion Temperature

    public static TaskDispatcher UserStateChanged { get; } = new();
    public static TaskDispatcher ProcessingStarted { get; } = new();
    public static TaskDispatcher ProcessingFinished { get; } = new();
}
