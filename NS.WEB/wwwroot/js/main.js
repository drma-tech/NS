window.browser = window.bowser.getParser(window.navigator.userAgent);

export const isBot =
    navigator.webdriver === true ||
    /google|baidu|bingbot|duckduckbot|teoma|slurp|yandex|toutiao|bytespider|applebot/i.test(
        navigator.userAgent
    );
export const isOldBrowser = window.browser.satisfies({
    chrome: "<131", //nov 2024 (avoid bots with fake browsers)
    //edge: "<131", //nov 2024
    //safari: "<18", //sep 2024
});
export const isLocalhost = location.host.includes("localhost");
export const isDev = location.hostname.includes("dev.");
export const isWebview = /webtonative/i.test(navigator.userAgent);
export const isPrintScreen = location.href.includes("printscreen");
export const appVersion = (await fetch("/build-date.txt").then(r => r.text())).trim();

export const servicesConfig = {
    AnalyticsCode: "",
    ClarityKey: "",
    UserBackToken: "A-A2J4M5NKCbDp1QyQe7ogemmmq",
    UserBackSurveyKey: "",
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

export const baseApiUrl = isLocalhost ? "http://localhost:7262" : "";
