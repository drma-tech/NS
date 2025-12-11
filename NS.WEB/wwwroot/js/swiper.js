import { Swiper } from "../package/swiper/swiper-bundle.min.mjs";

export const swiper = {
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
