window.browser = window.bowser.getParser(window.navigator.userAgent);

export const isBot =
    /google|baidu|bingbot|duckduckbot|teoma|slurp|yandex|toutiao|bytespider|applebot/i.test(
        navigator.userAgent
    );

// supports WebAssembly SIMD
export const isOldBrowser = window.browser.satisfies({
    chrome: "<91",
    edge: "<91",
    safari: "<16.4",
});
// validate only if it's a webapp
export const isBotBrowser = window.browser.satisfies({
    chrome: "<134", //feb 2025
});
export const isLocalhost = location.host.includes("localhost");
export const isDev = location.hostname.includes("dev.");
export const isWebview = /webtonative/i.test(navigator.userAgent);
export const isPrintScreen = location.href.includes("printscreen");
export let appVersion = "loading";

fetch("/build-date.txt")
    .then((r) => r.text())
    .then((text) => {
        appVersion = text.trim();
    }).catch(() => {
        appVersion = "version-error";
    });

export const servicesConfig = {
    AnalyticsCode: "G-G8CBVXZDD8",
    ClarityKey: "veo2fcozds",
    UserBackToken: "A-A2J4M5NKCbDp1QyQe7ogemmmq",
    UserBackSurveyKey: "",
    SentryDsn: "https://7ef8d637b418314a675a37d9cc70522c@o4510938040041472.ingest.us.sentry.io/4510943123668992",
};

export const firebaseConfig = {
    apiKey: "AIzaSyDdhjxD-5Kcgs2qtirEJ4WSUG-9oRaW_No",
    authDomain: "auth.my-next-spot.com",
    projectId: "my-next-spot",
    storageBucket: "my-next-spot.firebasestorage.app",
    messagingSenderId: "601130071474",
    messagingKey: "",
    appId: "1:601130071474:web:de4581b10d840a7dc860ab",
    measurementId: "G-G8CBVXZDD8",
};

export const supabaseConfig = {
    projectUrl: "",
    supabaseKey: "",
};

export const baseApiUrl = isLocalhost ? "http://localhost:7262" : "";

// Disable robots for dev environment
if (isDev) {
    const meta = document.createElement("meta");
    meta.name = "robots";
    meta.content = "noindex, nofollow";
    document.head.appendChild(meta);
}

// temporary: remove in the end of 2026
if (typeof Promise.withResolvers !== "function") {
    notification.showError("Your system’s web engine is outdated and may not support all features. Please update your device or browser to ensure the best experience.");
    Promise.withResolvers = function () {
        let resolve, reject;
        const promise = new Promise((res, rej) => {
            resolve = res;
            reject = rej;
        });
        return { promise, resolve, reject };
    };
}
