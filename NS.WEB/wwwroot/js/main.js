const ua = navigator.userAgent;

window.browser = window?.bowser?.getParser
    ? window.bowser.getParser(ua)
    : null;

export const isBot = /google|baidu|bingbot|duckduckbot|teoma|slurp|yandex|toutiao|bytespider|applebot/i.test(ua);

function testBrowserVersion(rules, ignore = false, fallback = false) {
    if (ignore) return false;

    if (!window.browser) return fallback;

    return window.browser.satisfies(rules);
}

//browser versions not compatible with SIMD (Some versions above for stability)
export const hideBlazorIndex = testBrowserVersion(
    {
        chrome: "<96", //nov 21
        edge: "<96", //nov 21
        firefox: "<96", //jan 22
        safari: "<16.6", //jul 23
        opera: "<82", //dec 21
    },
    /Mediapartners-Google/i.test(ua),
    false // uncertain environment → allow
);

//probably a bot, so doesnt support sw
export const disableServiceWorker = testBrowserVersion(
    {
        chrome: "<134",
    },
    false,
    true // uncertain environment → disable
);

export const isLocalhost = location.host.includes("localhost");
export const isDev = location.hostname.includes("dev.");
export const isWebview = /webtonative/i.test(ua);
export const isPrintScreen = location.href.includes("printscreen");

export const servicesConfig = {
    AnalyticsCode: "G-G8CBVXZDD8",
    ClarityKey: "veo2fcozds",
    UserBackToken: "A-A2J4M5NKCbDp1QyQe7ogemmmq",
    SentryDsn: "https://7ef8d637b418314a675a37d9cc70522c@o4510938040041472.ingest.us.sentry.io/4510943123668992",
};

export const supabaseConfig = {
    projectUrl: "https://svvydpwqfuoxovuabgap.supabase.co",
    supabaseKey: "sb_publishable_Pp1UvXpyiHBMeaY9wwywaQ_11-ou6_W",
};

export const baseApiUrl = isLocalhost ? "http://localhost:7262" : "";

// Disable robots for dev environment
if (isDev) {
    const meta = document.createElement("meta");
    meta.name = "robots";
    meta.content = "noindex, nofollow";
    document.head.appendChild(meta);
}
