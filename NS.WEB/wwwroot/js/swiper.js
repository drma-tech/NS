import { Swiper } from "../package/swiper/swiper-bundle.min.mjs";

export const swiper = {
    initLists(id, size) {
        let mobile = window.innerHeight < 600 || window.innerWidth < 600;

        const el = document.getElementById(id);
        if (!el) return;
        const posterSize = size ?? (mobile ? 98 : 128);
        const margin = 8;

        const temp = new Swiper(el, {
            slidesPerView: "auto",
            spaceBetween: mobile ? 4 : 8,
            breakpointsBase: "container",
            navigation: {
                nextEl: ".swiper-button-next",
                prevEl: ".swiper-button-prev",
            },
            pagination: {
                el: ".swiper-pagination",
                clickable: true,
            },
            breakpoints: {
                [250 - margin]: { slidesPerView: Math.floor(250 / posterSize) },
                [300 - margin]: { slidesPerView: Math.floor(300 / posterSize) },
                [350 - margin]: { slidesPerView: Math.floor(350 / posterSize) },
                [400 - margin]: { slidesPerView: Math.floor(400 / posterSize) },
                [500 - margin]: { slidesPerView: Math.floor(500 / posterSize) },
                [600 - margin]: { slidesPerView: Math.floor(600 / posterSize) },
                [700 - margin]: { slidesPerView: Math.floor(700 / posterSize) },
                [800 - margin]: { slidesPerView: Math.floor(800 / posterSize) },
                [1000 - margin]: { slidesPerView: Math.floor(1000 / posterSize) },
                [1200 - margin]: { slidesPerView: Math.floor(1200 / posterSize) },
                [1400 - margin]: { slidesPerView: Math.floor(1400 / posterSize) },
                [1600 - margin]: { slidesPerView: Math.floor(1600 / posterSize) },
                [2000 - margin]: { slidesPerView: Math.floor(2000 / posterSize) },
            },
        });
    },
    initNews(id) {
        const el = document.getElementById(id);
        if (!el) return;

        const progCircle = document.querySelector(".autoplay-progress svg");
        const progContent = document.querySelector(".autoplay-progress span");

        const temp = new Swiper(el, {
            centeredSlides: true,
            lazy: true,
            autoplay: {
                delay: 2500,
                disableOnInteraction: false,
            },
            navigation: {
                nextEl: ".swiper-button-next",
                prevEl: ".swiper-button-prev",
            },
            pagination: {
                el: ".swiper-pagination",
                clickable: true,
            },
            on: {
                autoplayTimeLeft(s, time, progress) {
                    progCircle.style.setProperty("--progress", 1 - progress);
                    progContent.textContent = `${Math.ceil(time / 1000)}s`;
                },
            },
        });
    },
};
