using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using NS.WEB.Modules.Subscription.Core;
using System.Globalization;
using System.Security.Claims;

namespace NS.WEB.Core;

public static class AppStateStatic
{
    public static string? Token { get; set; }
    public static bool IsAuthenticated { get; set; }
    public static bool IsPremiumUser { get; set; }
    public static ClaimsPrincipal? User { get; set; }
    public static string? UserId { get; set; }

    public static Breakpoint Breakpoint { get; set; } = Breakpoint.Xs;
    public static Action<Breakpoint>? BreakpointChanged { get; set; }
    public static Size Size { get; set; } = Size.Small;

    public static BrowserWindowSize? BrowserWindowSize { get; set; }
    public static Action<BrowserWindowSize>? BrowserWindowSizeChanged { get; set; }

    public static string? Version { get; set; }

    public static async Task<string> GetAppVersion(IJSRuntime js)
    {
        try
        {
            var vs = await js.Utils().GetAppVersion();

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

    public static async Task<Platform> GetPlatform(IJSRuntime js)
    {
        await _platformSemaphore.WaitAsync();
        try
        {
            if (_platform.HasValue)
            {
                return _platform.Value;
            }

            var cache = await js.Utils().GetLocalStorage("platform");

            if (cache.NotEmpty())
            {
                if (System.Enum.TryParse<Platform>(cache, true, out var platform) && System.Enum.IsDefined(platform))
                {
                    _platform = platform;
                }
                else
                {
                    _platform = Platform.webapp;
                    await js.Utils().SetLocalStorage("platform", _platform!.ToString()!);
                }
            }
            else
            {
                _platform = Platform.webapp;
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

    private static AppLanguage? _appLanguage;
    private static readonly SemaphoreSlim _appLanguageSemaphore = new(1, 1);

    public static async Task<AppLanguage> GetAppLanguage(IJSRuntime js)
    {
        await _appLanguageSemaphore.WaitAsync();
        try
        {
            if (_appLanguage.HasValue)
            {
                return _appLanguage.Value;
            }

            var cache = await js.Utils().GetLocalStorage("app-language");

            if (cache.NotEmpty())
            {
                _appLanguage = ConvertAppLanguage(cache);

                if (_appLanguage == null)
                {
                    _appLanguage = AppLanguage.en;
                    await js.Utils().SetLocalStorage("app-language", _appLanguage.ToString()!);
                }
            }
            else
            {
                var code = await js.Window().InvokeAsync<string>("eval", "navigator.language || navigator.userLanguage");
                code = code[..2].ToLowerInvariant();

                _appLanguage = ConvertAppLanguage(code) ?? AppLanguage.en;
                await js.Utils().SetLocalStorage("app-language", _appLanguage.ToString()!);
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

    private static AppLanguage? ConvertAppLanguage(string? code)
    {
        if (code.Empty()) return null;

        if (System.Enum.TryParse<AppLanguage>(code, true, out var language) && System.Enum.IsDefined(language))
            return language;
        else
            return null;
    }

    #endregion AppLanguage

    #region DarkMode

    public static Action<bool>? DarkModeChanged { get; set; }

    private static bool? _darkMode;
    private static readonly SemaphoreSlim _darkModeSemaphore = new(1, 1);

    public static async Task<bool?> GetDarkMode(IJSRuntime js)
    {
        await _darkModeSemaphore.WaitAsync();
        try
        {
            if (_darkMode.HasValue)
            {
                return _darkMode.Value;
            }

            _darkMode = await js.Utils().GetLocalStorage<bool?>("dark-mode");

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

    public static async Task<string> GetCountry(IpInfoApi api, IpInfoServerApi serverApi, IJSRuntime js)
    {
        await _countrySemaphore.WaitAsync();
        try
        {
            if (_country.NotEmpty())
            {
                return _country;
            }

            var cache = await js.Utils().GetLocalStorage("country");

            if (cache.NotEmpty())
            {
                _country = cache.Trim();
            }
            else
            {
                _country = (await api.GetCountry())?.Trim();
                if (_country.NotEmpty()) await js.Utils().SetLocalStorage("country", _country.ToLower());
            }

            _country ??= "US";

            return _country;
        }
        catch
        {
            try
            {
                //if user country blocks external requests, try server side
                _country = (await serverApi.GetCountry())?.Trim();
                if (_country.NotEmpty()) await js.Utils().SetLocalStorage("country", _country.ToLower());

                _country ??= "US";

                return _country;
            }
            catch
            {
                _country = "US";
                return _country;
            }
        }
        finally
        {
            _countrySemaphore.Release();
        }
    }

    #endregion Region Country

    public static Action<string?>? AuthChanged { get; set; }
    public static Action<string, string>? NotificationEnabled { get; set; }
    public static Action? UserStateChanged { get; set; }
    public static Action? RegistrationSuccessful { get; set; }
    public static Action<string>? AppleVerify { get; set; }
    public static Action<string>? ShowError { get; set; }
    public static Action? ProcessingStarted { get; set; }
    public static Action? ProcessingFinished { get; set; }

    public static int TotalEnergy { get; set; } = 10;
    public static int ConsumedEnergy { get; set; } = 0;
    public static Action? EnergyConsumed { get; set; }
}
