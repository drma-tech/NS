using Microsoft.JSInterop;
using NS.WEB.Shared;
using System.Text.Json;

namespace NS.WEB.Core.Helper
{
    public static class JsModuleLoader
    {
        private static readonly Dictionary<string, IJSObjectReference> cache = [];

        public static async Task<IJSObjectReference> Load(IJSRuntime js, string path)
        {
            if (!cache.TryGetValue(path, out var module))
            {
                module = await js.InvokeAsync<IJSObjectReference>("import", path);
                cache[path] = module;
            }

            return module;
        }
    }

    public abstract class JsModuleBase(IJSRuntime js, string path)
    {
        protected async Task InvokeVoid(string method, params object?[] args)
        {
            var module = await JsModuleLoader.Load(js, path);
            await module.InvokeVoidAsync(method, args);
        }

        protected async Task<T> Invoke<T>(string method, params object?[] args)
        {
            var module = await JsModuleLoader.Load(js, path);
            return await module.InvokeAsync<T>(method, args);
        }
    }

    public static class JsModules
    {
        public static WindowJs Window(this IJSRuntime js) => new(js);

        public static UtilsJs Utils(this IJSRuntime js) => new(js);

        public static FirebaseJs Firebase(this IJSRuntime js) => new(js);

        public static ServicesJs Services(this IJSRuntime js) => new(js);

        public static SwiperJs Swiper(this IJSRuntime js) => new(js);

        public static PaymentsJs Payments(this IJSRuntime js) => new(js);
    }

    public class WindowJs(IJSRuntime js)
    {
        public async Task HistoryBack() => await js.InvokeVoidAsync("history.back");

        public async Task InvokeVoidAsync(string identifier, params object?[]? args) => await js.InvokeVoidAsync(identifier, args);

        public async Task<T> InvokeAsync<T>(string identifier, params object?[]? args) => await js.InvokeAsync<T>(identifier, args);
    }

    public class UtilsJs(IJSRuntime js) : JsModuleBase(js, "./js/utils.js")
    {
        #region STORAGE

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public enum BrowserStorageType
        {
            Local,
            Session
        }

        /// <summary>
        /// used for string, class, collections, etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        /// <exception cref="UnhandledException"></exception>
        public async Task<T?> GetStorage<T>(string key, BrowserStorageType storage = BrowserStorageType.Local)
        {
            var value = await Invoke<string?>(storage == BrowserStorageType.Local ? "storage.getLocalStorage" : "storage.getSessionStorage", key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }
            else if (typeof(T).IsEnum)
            {
                throw new UnhandledException("GetStorage cannot be used with enum");
            }
            else
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(value);
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// used for structs, bool, enum, datetime etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public async Task<T?> GetStorageStruct<T>(string key, BrowserStorageType storage = BrowserStorageType.Local) where T : struct
        {
            var value = await Invoke<string?>(storage == BrowserStorageType.Local ? "storage.getLocalStorage" : "storage.getSessionStorage", key);

            if (value.Empty())
            {
                return default;
            }
            else if (typeof(T).IsEnum)
            {
                return Enum.TryParse(typeof(T), value, ignoreCase: true, out var enumValue) && Enum.IsDefined(typeof(T), enumValue) ? (T)enumValue : null;
            }
            else
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(value, JsonSerializerOptions);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Task SetStorage<T>(string key, T value, BrowserStorageType storage = BrowserStorageType.Local)
        {
            if (value is null) return RemoveStorage(storage, key);

            string storedValue;
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);

            if (value is string s)
            {
                storedValue = s.ToLowerInvariant();
            }
            else if (type.IsEnum || (underlyingType != null && underlyingType.IsEnum))
            {
                storedValue = value.ToString()?.ToLowerInvariant() ?? throw new UnhandledException("invalid enum value");
            }
            else
            {
                storedValue = JsonSerializer.Serialize(value, JsonSerializerOptions);
            }

            return InvokeVoid(storage == BrowserStorageType.Local ? "storage.setLocalStorage" : "storage.setSessionStorage", key, storedValue);
        }

        public Task RemoveStorage(BrowserStorageType storage, string key)
        {
            return InvokeVoid(storage == BrowserStorageType.Local ? "storage.removeLocalStorage" : "storage.removeSessionStorage", key);
        }

        public Task ShowCache() => InvokeVoid("storage.showCache");

        public Task ClearAllStorage() => InvokeVoid("storage.clearAllStorage");

        #endregion STORAGE

        #region NOTIFICATION

        public Task<string?> PlayBeep(int frequency, int duration, string type) => Invoke<string?>("notification.playBeep", frequency, duration, type);

        public Task<string?> Vibrate(int[] pattern) => Invoke<string?>("notification.vibrate", pattern);

        #endregion NOTIFICATION

        #region ENVIRONMENT

        public Task<string?> GetAppVersion() => Invoke<string?>("environment.getAppVersion");

        #endregion ENVIRONMENT

        #region INTEROP

        public Task DownloadFile(string filename, string contentType, object? content) => InvokeVoid("interop.downloadFile", filename, contentType, content);

        #endregion INTEROP
    }

    public class FirebaseJs(IJSRuntime js) : JsModuleBase(js, "./js/firebase.js")
    {
        public async Task SignInAsync(string providerName)
        {
            ApiCore.ResetCacheVersion();
            await InvokeVoid("authentication.signIn", providerName);
        }

        public Task SignOutAsync() => InvokeVoid("authentication.signOut");
    }

    public class ServicesJs(IJSRuntime js) : JsModuleBase(js, "./js/services.js")
    {
        public Task InitGoogleAnalytics(string version) => InvokeVoid("services.initGoogleAnalytics", version);

        public Task InitUserBack(string version) => InvokeVoid("services.initUserBack", version);

        public Task InitAdSense(string adClient, GoogleAdSense.AdUnit adSlot, string? adFormat, string containerId) => InvokeVoid("services.initAdSense", adClient, ((long)adSlot).ToString(), adFormat, containerId);
    }

    public class SwiperJs(IJSRuntime js) : JsModuleBase(js, "./js/swiper.js")
    {
        public Task InitLists(string id, int? size = null) => InvokeVoid("swiper.initLists", id, size);

        public Task InitNews(string id) => InvokeVoid("swiper.initNews", id);
    }

    public class PaymentsJs(IJSRuntime js) : JsModuleBase(js, "./js/payments.js")
    {
        public Task AppleOpenCheckout(string? productId) => InvokeVoid("apple.openCheckout", productId);

        public Task GoogleOpenCheckout(string? productId, string type) => InvokeVoid("google.openCheckout", productId, type);

        public Task StripeOpenCheckout(string? priceId) => InvokeVoid("stripe.openCheckout", priceId);
    }
}