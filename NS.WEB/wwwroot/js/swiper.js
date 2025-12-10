import { Swiper } from "../package/swiper/swiper-bundle.min.mjs";

export const swiper = {
    initNews(id) {
        const el = document.getElementById(id);
        if (!el) return;

        const progressCircle = document.querySelector(".autoplay-progress svg");
        const progressContent = document.querySelector(".autoplay-progress span");

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
                    progressCircle.style.setProperty(
                        "--progress",
                        1 - progress
                    );
                    progressContent.textContent = `${Math.ceil(time / 1000)}s`;
                },
            },
        });
    },
};
